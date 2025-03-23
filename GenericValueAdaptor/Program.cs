using System;
using System.Collections.Generic;

// Generic adapter interface
interface IValueAdapter<TInput, TOutput>
{
    TOutput Convert(TInput input);
}

// Adapter: Converts int to string
class IntToStringAdapter : IValueAdapter<int, string>
{
    public string Convert(int input)
    {
        return $"Value: {input}";
    }
}

// Adapter: Converts string to double
class StringToDoubleAdapter : IValueAdapter<string, double>
{
    public double Convert(string input)
    {
        if (double.TryParse(input, out double result))
            return result;

        throw new FormatException("Invalid double format");
    }
}

// Adapter: Converts Celsius to Fahrenheit
class CelsiusToFahrenheitAdapter : IValueAdapter<double, double>
{
    public double Convert(double input)
    {
        return (input * 9 / 5) + 32;
    }
}

// Client usage
class Program
{
    static void Main()
    {
        // Using different adapters
        IValueAdapter<int, string> intToString = new IntToStringAdapter();
        IValueAdapter<string, double> stringToDouble = new StringToDoubleAdapter();
        IValueAdapter<double, double> celsiusToFahrenheit = new CelsiusToFahrenheitAdapter();

        // Sample values
        int number = 42;
        string numericString = "3.14";
        double celsius = 25.0;

        // Convert values
        Console.WriteLine(intToString.Convert(number));           // Output: "Value: 42"
        Console.WriteLine(stringToDouble.Convert(numericString)); // Output: 3.14
        Console.WriteLine(celsiusToFahrenheit.Convert(celsius));  // Output: 77°F
        Console.ReadLine();
    }
}
