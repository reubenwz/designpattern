using System;
using DryIoc;

// Step 1: Define a common interface for telemetry data retrieval
public interface ITelemetryAdapter
{
    string GetTelemetry();
}

// Step 2: Implement an adapter that fetches telemetry from a database
public class DatabaseTelemetryAdapter : ITelemetryAdapter
{
    public string GetTelemetry()
    {
        return "Telemetry from Database: {Battery: 95%, Temperature: 22°C}";
    }
}

// Step 3: Implement another adapter that fetches telemetry from a live API
public class ApiTelemetryAdapter : ITelemetryAdapter
{
    public string GetTelemetry()
    {
        return "Telemetry from API: {Battery: 92%, Temperature: 21°C}";
    }
}

// Step 4: Spacecraft that uses the adapter for telemetry retrieval
public class Spacecraft
{
    private readonly ITelemetryAdapter _telemetryAdapter;

    public Spacecraft(ITelemetryAdapter telemetryAdapter)
    {
        _telemetryAdapter = telemetryAdapter;
    }

    public void ShowTelemetry()
    {
        Console.WriteLine($"Spacecraft telemetry: {_telemetryAdapter.GetTelemetry()}");
    }
}

// Step 5: Main program with DryIoc container
class Program
{
    static void Main()
    {
        // Create DryIoc container
        var container = new Container();

        // Register the telemetry adapter (change between DatabaseTelemetryAdapter or ApiTelemetryAdapter)
        container.Register<ITelemetryAdapter, DatabaseTelemetryAdapter>(Reuse.Singleton);
        // container.Register<ITelemetryAdapter, ApiTelemetryAdapter>(Reuse.Singleton); // Switch if needed

        // Register the spacecraft with injected adapter
        container.Register<Spacecraft>();

        // Resolve Spacecraft and use it
        var spacecraft = container.Resolve<Spacecraft>();
        spacecraft.ShowTelemetry();
    }
}
