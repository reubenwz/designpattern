using System;

// Step 1: Define the telemetry system interface (Implementation Hierarchy)
public interface ITelemetrySystem
{
    string GetTelemetry();
}

// Step 2: Concrete implementations of telemetry systems
public class DatabaseTelemetry : ITelemetrySystem
{
    public string GetTelemetry() => "Telemetry from Database: {Battery: 95%, Temperature: 22°C}";
}

public class ApiTelemetry : ITelemetrySystem
{
    public string GetTelemetry() => "Telemetry from API: {Battery: 92%, Temperature: 21°C}";
}

// Step 3: Define the spacecraft abstraction (Abstraction Hierarchy)
public abstract class Spacecraft
{
    protected ITelemetrySystem _telemetrySystem;

    // Constructor takes an implementation of ITelemetrySystem
    protected Spacecraft(ITelemetrySystem telemetrySystem)
    {
        _telemetrySystem = telemetrySystem;
    }

    public abstract void ShowTelemetry();
}

// Step 4: Concrete spacecraft implementations
public class OrbitalSpacecraft : Spacecraft
{
    public OrbitalSpacecraft(ITelemetrySystem telemetrySystem) : base(telemetrySystem) { }

    public override void ShowTelemetry()
    {
        Console.WriteLine($"Orbital Spacecraft telemetry: {_telemetrySystem.GetTelemetry()}");
    }
}

public class LunarLander : Spacecraft
{
    public LunarLander(ITelemetrySystem telemetrySystem) : base(telemetrySystem) { }

    public override void ShowTelemetry()
    {
        Console.WriteLine($"Lunar Lander telemetry: {_telemetrySystem.GetTelemetry()}");
    }
}

// Step 5: Usage
class Program
{
    static void Main()
    {
        // Create telemetry systems
        ITelemetrySystem databaseTelemetry = new DatabaseTelemetry();
        ITelemetrySystem apiTelemetry = new ApiTelemetry();

        // Create spacecrafts with different telemetry systems
        Spacecraft orbitalSpacecraft = new OrbitalSpacecraft(databaseTelemetry);
        Spacecraft lunarLander = new LunarLander(apiTelemetry);

        // Show telemetry
        orbitalSpacecraft.ShowTelemetry();
        lunarLander.ShowTelemetry();
    }
}
