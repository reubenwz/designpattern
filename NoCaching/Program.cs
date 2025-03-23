using System;

// Step 1: Define the target interface (what the client expects)
interface ITemperatureService
{
    double GetTemperatureInFahrenheit();
}

// Step 2: Existing class (incompatible with the target interface)
class CelsiusThermometer
{
    public double GetTemperatureInCelsius()
    {
        return 25.0; // Simulated temperature in Celsius
    }
}

// Step 3: Adapter class implementing the target interface
class TemperatureAdapter : ITemperatureService
{
    private readonly CelsiusThermometer _thermometer;

    public TemperatureAdapter(CelsiusThermometer thermometer)
    {
        _thermometer = thermometer;
    }

    // Converts Celsius to Fahrenheit
    public double GetTemperatureInFahrenheit()
    {
        double celsius = _thermometer.GetTemperatureInCelsius();
        return (celsius * 9 / 5) + 32;
    }
}

// Step 4: Client code using the target interface
class Program
{
    static void Main()
    {
        // Using the existing class but wrapped in an adapter
        CelsiusThermometer legacyThermometer = new CelsiusThermometer();
        ITemperatureService temperatureService = new TemperatureAdapter(legacyThermometer);

        Console.WriteLine($"Temperature in Fahrenheit: {temperatureService.GetTemperatureInFahrenheit()}°F");
    }
}
