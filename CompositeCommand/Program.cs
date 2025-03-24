using System;
using System.Collections.Generic;

// ====================== 1. Core Models ======================
public class MissionPlan
{
    public double PowerConsumption { get; set; }
    public double MaxTemperature { get; set; }
}

public class SatelliteState
{
    public double AvailablePower { get; set; }
    public double MaxAllowedTemperature { get; set; }
}

// ====================== 2. Command Interface ======================
public interface IValidationCommand
{
    string Name { get; }
    bool Execute(MissionPlan plan);
    void Undo();
}

// ====================== 3. Individual Commands ======================
public class PowerCheckCommand : IValidationCommand
{
    private readonly SatelliteState _satellite;
    public string Name => "Power Check";

    public PowerCheckCommand(SatelliteState satellite) => _satellite = satellite;

    public bool Execute(MissionPlan plan)
    {
        bool isValid = plan.PowerConsumption <= _satellite.AvailablePower;
        Console.WriteLine($"{Name}: {(isValid ? "PASS" : "FAIL")}");
        return isValid;
    }

    public void Undo() => Console.WriteLine($"{Name}: Releasing reserved power...");
}

public class ThermalCheckCommand : IValidationCommand
{
    private readonly SatelliteState _satellite;
    public string Name => "Thermal Check";

    public ThermalCheckCommand(SatelliteState satellite) => _satellite = satellite;

    public bool Execute(MissionPlan plan)
    {
        bool isValid = plan.MaxTemperature <= _satellite.MaxAllowedTemperature;
        Console.WriteLine($"{Name}: {(isValid ? "PASS" : "FAIL")}");
        return isValid;
    }

    public void Undo() => Console.WriteLine($"{Name}: Cooling system reset...");
}

// ====================== 4. Composite Command ======================
public class CompositeValidationCommand : IValidationCommand
{
    private readonly List<IValidationCommand> _commands = new();
    public string Name => "Composite Validation";

    public void AddCommand(IValidationCommand command) => _commands.Add(command);

    public bool Execute(MissionPlan plan)
    {
        Console.WriteLine($"\nStarting {Name} ({_commands.Count} checks)");
        bool allPassed = true;
        foreach (var cmd in _commands)
        {
            if (!cmd.Execute(plan))
            {
                allPassed = false;
                break; // Fail-fast (or remove to continue)
            }
        }
        Console.WriteLine($"{Name}: {(allPassed ? "ALL PASSED" : "FAILED")}");
        return allPassed;
    }

    public void Undo()
    {
        Console.WriteLine($"\nUndoing {Name}");
        for (int i = _commands.Count - 1; i >= 0; i--) // Reverse undo order
        {
            _commands[i].Undo();
        }
    }
}

// ====================== 5. Invoker ======================
public class ValidationInvoker
{
    private readonly Queue<IValidationCommand> _queue = new();
    private readonly Stack<IValidationCommand> _executed = new();

    public void Enqueue(IValidationCommand command) => _queue.Enqueue(command);

    public bool RunAll(MissionPlan plan)
    {
        Console.WriteLine("\n=== Running Validation Queue ===");
        while (_queue.TryDequeue(out var cmd))
        {
            bool success = cmd.Execute(plan);
            _executed.Push(cmd);

            if (!success)
            {
                Console.WriteLine("[ABORTING: Validation Failed]");
                return false;
            }
        }
        Console.WriteLine("[QUEUE COMPLETE: All Validations Passed]");
        return true;
    }

    public void UndoAll()
    {
        Console.WriteLine("\n=== Undoing All Operations ===");
        while (_executed.TryPop(out var cmd))
        {
            cmd.Undo();
        }
    }
}

// ====================== 6. Main Program ======================
class Program
{
    static void Main()
    {
        var plan = new MissionPlan { PowerConsumption = 1500, MaxTemperature = 310 };
        var satellite = new SatelliteState { AvailablePower = 2000, MaxAllowedTemperature = 320 };

        // Create individual commands
        var powerCheck = new PowerCheckCommand(satellite);
        var thermalCheck = new ThermalCheckCommand(satellite);

        // Create a composite command (group of checks)
        var systemChecks = new CompositeValidationCommand();
        systemChecks.AddCommand(powerCheck);
        systemChecks.AddCommand(thermalCheck);

        // Create another composite (nested)
        var safetyChecks = new CompositeValidationCommand();
        safetyChecks.AddCommand(new ThermalCheckCommand(satellite)); // Duplicate for demo

        // Configure invoker
        var invoker = new ValidationInvoker();
        invoker.Enqueue(systemChecks); // Composite command
        invoker.Enqueue(safetyChecks); // Another composite
        invoker.Enqueue(new PowerCheckCommand(satellite)); // Individual command

        // Run everything
        bool isValid = invoker.RunAll(plan);

        // Undo if needed
        if (!isValid)
        {
            invoker.UndoAll();
        }
    }
}