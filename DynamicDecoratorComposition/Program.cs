using System;
using System.Collections.Generic;

namespace ValidationEngineDecorator
{
    // Step 1: Define the Base Interface
    public interface IMissionValidator
    {
        bool Validate(MissionPlan mission);
    }

    // Step 2: Create the Concrete Component (Base Validator)
    public class BasicMissionValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
        {
            Console.WriteLine("Basic validation passed.");
            return true;
        }
    }

    // Step 3: Create the Decorator Base Class
    public abstract class MissionValidatorDecorator : IMissionValidator
    {
        public abstract bool Validate(MissionPlan mission);
    }

    // Time Window Constraint
    public class TimeWindowValidator : MissionValidatorDecorator
    {
        public override bool Validate(MissionPlan mission)
        {
            if (mission.StartTime.Hour < 6 || mission.EndTime.Hour > 22)
            {
                Console.WriteLine("Mission rejected: Outside allowed time window.");
                return false;
            }

            Console.WriteLine("Time Window Constraint passed.");
            return true;
        }
    }

    // Altitude Constraint
    public class AltitudeValidator : MissionValidatorDecorator
    {
        public override bool Validate(MissionPlan mission)
        {
            if (mission.Altitude < 500 || mission.Altitude > 1000)
            {
                Console.WriteLine("Mission rejected: Altitude out of safe range.");
                return false;
            }

            Console.WriteLine("Altitude Constraint passed.");
            return true;
        }
    }

    // Power Constraint
    public class PowerValidator : MissionValidatorDecorator
    {
        public override bool Validate(MissionPlan mission)
        {
            if (mission.PowerConsumption > 100)
            {
                Console.WriteLine("Mission rejected: Power consumption too high.");
                return false;
            }

            Console.WriteLine("Power Constraint passed.");
            return true;
        }
    }

    // Step 4: Create a Composite Validator for Dynamic Composition
    public class CompositeMissionValidator : IMissionValidator
    {
        private readonly List<IMissionValidator> _validators = new List<IMissionValidator>();

        public void AddValidator(IMissionValidator validator)
        {
            _validators.Add(validator);
        }

        public bool Validate(MissionPlan mission)
        {
            foreach (var validator in _validators)
            {
                if (!validator.Validate(mission))
                {
                    return false; // Stop if any validator fails
                }
            }
            return true;
        }
    }

    // Mission Plan Class
    public class MissionPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Altitude { get; set; }
        public int PowerConsumption { get; set; }
    }

    // Step 5: Main Program
    class Program
    {
        static void Main()
        {
            var mission = new MissionPlan
            {
                StartTime = new DateTime(2025, 5, 10, 8, 0, 0),
                EndTime = new DateTime(2025, 5, 10, 21, 0, 0),
                Altitude = 600,
                PowerConsumption = 90
            };

            Console.WriteLine("Validating Mission Plan...\n");

            // Create composite validator (manages multiple constraints dynamically)
            var compositeValidator = new CompositeMissionValidator();
            compositeValidator.AddValidator(new BasicMissionValidator());
            compositeValidator.AddValidator(new TimeWindowValidator());
            compositeValidator.AddValidator(new AltitudeValidator());
            compositeValidator.AddValidator(new PowerValidator());

            // Run validation
            bool isValid = compositeValidator.Validate(mission);

            Console.WriteLine($"\nMission Validation Result: {(isValid ? "Approved" : "Rejected")}");
            Console.ReadLine(); // Keep console open
        }
    }
}
