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
        // Simulate reading temperature (random value for testing)
        return new Random().Next(20, 30); // Random temperature between 20°C and 30°C
    }
}

// Step 3: Adapter with caching
class TemperatureAdapter : ITemperatureService
{
    private readonly CelsiusThermometer _thermometer;
    private double _lastCelsius = double.MinValue;
    private double _cachedFahrenheit = double.MinValue;

    public TemperatureAdapter(CelsiusThermometer thermometer)
    {
        _thermometer = thermometer;
    }

    public double GetTemperatureInFahrenheit()
    {
        double currentCelsius = _thermometer.GetTemperatureInCelsius();

        // Check if temperature has changed
        if (currentCelsius != _lastCelsius)
        {
            _lastCelsius = currentCelsius;
            _cachedFahrenheit = (currentCelsius * 9 / 5) + 32; // Convert to Fahrenheit
            Console.WriteLine($"[LOG] Converted {currentCelsius}°C to {_cachedFahrenheit}°F");
        }
        else
        {
            Console.WriteLine("[LOG] Using cached temperature.");
        }

        return _cachedFahrenheit;
    }
}

// Step 4: Client code using the target interface
class Program
{
    static void Main()
    {
        CelsiusThermometer legacyThermometer = new CelsiusThermometer();
        ITemperatureService temperatureService = new TemperatureAdapter(legacyThermometer);

        // Simulate multiple requests
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Temperature in Fahrenheit: {temperatureService.GetTemperatureInFahrenheit()}°F");
            System.Threading.Thread.Sleep(5000); // Simulate process delay
        }
        Console.ReadLine();
    }
}
