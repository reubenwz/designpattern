using System;
using DryIoc;

// Step 1: Define the Spacecraft interface
public interface ISpacecraft
{
    string Name { get; }
    double OrbitAltitude { get; }
    double FuelLevel { get; set; }

    void Refuel();
}

// Step 2: Implement the Spacecraft class
public class Spacecraft : ISpacecraft
{
    public string Name { get; }
    public double OrbitAltitude { get; }
    public double FuelLevel { get; set; }

    public Spacecraft()
    {
        Name = "TeLEOS-2";
        OrbitAltitude = 550; // km
        FuelLevel = 100; // Fully fueled
    }

    public void Refuel()
    {
        FuelLevel = 100;
        Console.WriteLine($"{Name} has been refueled!");
    }
}

// Step 3: Main program with DryIoc container
class Program
{
    static void Main()
    {
        // Create DryIoc container
        var container = new Container();

        // Register ISpacecraft as a Singleton
        container.Register<ISpacecraft, Spacecraft>(Reuse.Singleton);

        // Resolve instances
        var spacecraft1 = container.Resolve<ISpacecraft>();
        var spacecraft2 = container.Resolve<ISpacecraft>();

        // Modify the spacecraft instance via one reference
        spacecraft1.FuelLevel = 50;

        // Check if the second reference reflects the changes (singleton behavior)
        Console.WriteLine($"Spacecraft 1 Fuel Level: {spacecraft1.FuelLevel}%");
        Console.WriteLine($"Spacecraft 2 Fuel Level: {spacecraft2.FuelLevel}%");

        Console.WriteLine(spacecraft1 == spacecraft2
            ? "Singleton works! Both references point to the same instance."
            : "Singleton failed! Different instances were created.");
    }
}
