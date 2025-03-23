using System;
using System.Collections.Generic;

// Step 1: Define the component interface
public interface ISpacecraftComponent
{
    void ShowTelemetry();
}

// Step 2: Leaf components (individual sensors)
public class Sensor : ISpacecraftComponent
{
    private string _name;
    private string _data;

    public Sensor(string name, string data)
    {
        _name = name;
        _data = data;
    }

    public void ShowTelemetry()
    {
        Console.WriteLine("Sensor: " + _name + ", Data: " + _data);
    }
}

// Step 3: Composite component (module containing multiple sensors)
public class SatelliteModule : ISpacecraftComponent
{
    private string _moduleName;
    private List<ISpacecraftComponent> _components;

    public SatelliteModule(string moduleName)
    {
        _moduleName = moduleName;
        _components = new List<ISpacecraftComponent>();
    }

    public void AddComponent(ISpacecraftComponent component)
    {
        _components.Add(component);
    }

    public void ShowTelemetry()
    {
        Console.WriteLine("--- Module: " + _moduleName + " ---");
        foreach (var component in _components)
        {
            component.ShowTelemetry();
        }
    }
}

// Step 4: Usage in Main()
class Program
{
    static void Main()
    {
        // Create individual sensors (leaves)
        ISpacecraftComponent tempSensor = new Sensor("Temperature Sensor", "22°C");
        ISpacecraftComponent batterySensor = new Sensor("Battery Sensor", "95%");

        // Create a module and add sensors to it
        SatelliteModule powerModule = new SatelliteModule("Power Module");
        powerModule.AddComponent(tempSensor);
        powerModule.AddComponent(batterySensor);

        // Create another sensor
        ISpacecraftComponent radiationSensor = new Sensor("Radiation Sensor", "Normal");

        // Create spacecraft and add modules/sensors
        SatelliteModule spacecraft = new SatelliteModule("TeLEOS-2 Spacecraft");
        spacecraft.AddComponent(powerModule);
        spacecraft.AddComponent(radiationSensor);

        // Display telemetry
        spacecraft.ShowTelemetry();
    }
}
   