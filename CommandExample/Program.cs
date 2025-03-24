using System;
using System.Collections.Generic;

// ====================== 1. Core Data Models ======================
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

// ====================== 3. Concrete Commands ======================
public class PowerCheckCommand : IValidationCommand
{
    private SatelliteState _satellite;
    public string Name => "Power Check";

    public PowerCheckCommand(SatelliteState satellite)
    {
        _satellite = satellite;
    }

    public bool Execute(MissionPlan plan)
    {
        bool isValid = plan.PowerConsumption <= _satellite.AvailablePower;
        Console.WriteLine($"{Name}: {(isValid ? "PASS" : "FAIL")}");
        return isValid;
    }

    public void Undo() => Console.WriteLine($"{Name}: Undoing power reservation...");
}

public class ThermalCheckCommand : IValidationCommand
{
    private SatelliteState _satellite;
    public string Name => "Thermal Check";

    public ThermalCheckCommand(SatelliteState satellite)
    {
        _satellite = satellite;
    }

    public bool Execute(MissionPlan plan)
    {
        bool isValid = plan.MaxTemperature <= _satellite.MaxAllowedTemperature;
        Console.WriteLine($"{Name}: {(isValid ? "PASS" : "FAIL")}");
        return isValid;
    }

    public void Undo() => Console.WriteLine($"{Name}: Reverting thermal adjustments...");
}

// ====================== 4. Invoker (Validation Engine) ======================
public class ValidationEngine
{
    private readonly Queue<IValidationCommand> _commandQueue = new();
    private readonly Stack<IValidationCommand> _executedCommands = new();

    public void AddCommand(IValidationCommand command)
    {
        _commandQueue.Enqueue(command);
    }

    public bool RunAll(MissionPlan plan)
    {
        Console.WriteLine("\n=== Running Validations ===");
        while (_commandQueue.TryDequeue(out var cmd))
        {
            bool success = cmd.Execute(plan);
            _executedCommands.Push(cmd);

            if (!success)
            {
                Console.WriteLine("[VALIDATION FAILED]");
                return false;
            }
        }
        Console.WriteLine("[ALL CHECKS PASSED]");
        return true;
    }

    public void UndoAll()
    {
        Console.WriteLine("\n=== Undoing Changes ===");
        while (_executedCommands.TryPop(out var cmd))
        {
            cmd.Undo();
        }
    }
}

// ====================== 5. Main Program ======================
class Program
{
    static void Main()
    {
        // Setup test data
        var missionPlan = new MissionPlan
        {
            PowerConsumption = 1500,
            MaxTemperature = 310
        };

        var satellite = new SatelliteState
        {
            AvailablePower = 2000,
            MaxAllowedTemperature = 320
        };

        // Configure validation engine
        var engine = new ValidationEngine();
        engine.AddCommand(new PowerCheckCommand(satellite));
        engine.AddCommand(new ThermalCheckCommand(satellite));

        // Run validations
        bool isValid = engine.RunAll(missionPlan);

        // Undo if validation failed
        if (!isValid)
        {
            engine.UndoAll();
        }

        // Demo queuing: Add a new check and rerun
        Console.WriteLine("\n=== Adding New Check ===");
        engine.AddCommand(new ThermalCheckCommand(satellite));
        engine.RunAll(missionPlan);
    }
}