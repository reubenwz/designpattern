using System;
using System.Collections.Generic;

namespace MediatorPatternValidation
{
    // Step 1: Define the Mediator Interface
    public interface IMissionValidationMediator
    {
        void RegisterValidator(IMissionValidator validator);
        bool ValidateMission(MissionPlan mission);
    }

    // Step 2: Concrete Mediator
    public class MissionValidationMediator : IMissionValidationMediator
    {
        private readonly List<IMissionValidator> _validators = new List<IMissionValidator>();

        public void RegisterValidator(IMissionValidator validator)
        {
            _validators.Add(validator);
        }

        public bool ValidateMission(MissionPlan mission)
        {
            foreach (var validator in _validators)
            {
                if (!validator.Validate(mission))
                {
                    Console.WriteLine("Mission rejected due to validation failure.");
                    return false;
                }
            }
            Console.WriteLine("Mission Approved!");
            return true;
        }
    }

    // Step 3: Define the Validator Interface
    public interface IMissionValidator
    {
        bool Validate(MissionPlan mission);
    }

    // Step 4: Concrete Validators

    public class TimeWindowValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
        {
            if (mission.StartTime.Hour < 6 || mission.EndTime.Hour > 22)
            {
                Console.WriteLine("Time constraint failed.");
                return false;
            }
            Console.WriteLine("Time constraint passed.");
            return true;
        }
    }

    public class AltitudeValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
        {
            if (mission.Altitude < 500 || mission.Altitude > 1000)
            {
                Console.WriteLine("Altitude constraint failed.");
                return false;
            }
            Console.WriteLine("Altitude constraint passed.");
            return true;
        }
    }

    public class PowerValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
        {
            if (mission.PowerConsumption > 100)
            {
                Console.WriteLine("Power constraint failed.");
                return false;
            }
            Console.WriteLine("Power constraint passed.");
            return true;
        }
    }

    // Step 5: Define the Mission Plan
    public class MissionPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Altitude { get; set; }
        public int PowerConsumption { get; set; }
    }

    // Step 6: Run the Example
    class Program
    {
        static void Main()
        {
            MissionValidationMediator mediator = new MissionValidationMediator();

            // Register validators
            mediator.RegisterValidator(new TimeWindowValidator());
            mediator.RegisterValidator(new AltitudeValidator());
            mediator.RegisterValidator(new PowerValidator());

            // Create a mission
            MissionPlan mission = new MissionPlan
            {
                StartTime = new DateTime(2025, 5, 10, 8, 0, 0),
                EndTime = new DateTime(2025, 5, 10, 21, 0, 0),
                Altitude = 600,
                PowerConsumption = 90
            };

            // Validate the mission through the mediator
            mediator.ValidateMission(mission);

            Console.ReadLine();
        }
    }
}
