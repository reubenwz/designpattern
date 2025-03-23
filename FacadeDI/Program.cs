using System;
using DryIoc;

namespace FacadePatternValidationWithDI
{
    // Step 1: Define the MissionPlan class
    public class MissionPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Altitude { get; set; }
        public int PowerConsumption { get; set; }
    }

    // Step 2: Define Validator Interfaces
    public interface IMissionValidator
    {
        bool Validate(MissionPlan mission);
    }

    // Step 3: Implement Concrete Validators
    public class TimeWindowValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
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

    public class AltitudeValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
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

    public class PowerValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
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

    // Step 4: Create the Facade with Dependency Injection
    public class MissionValidationFacade
    {
        private readonly IMissionValidator[] _validators;

        public MissionValidationFacade(IMissionValidator[] validators)
        {
            _validators = validators;
        }

        public bool ValidateMission(MissionPlan mission)
        {
            Console.WriteLine("Starting mission validation...");

            foreach (var validator in _validators)
            {
                if (!validator.Validate(mission))
                {
                    return false; // Fail fast if any validation fails
                }
            }
            return true;
        }
    }

    // Step 5: Configure DryIoc and Run the Program
    class Program
    {
        static void Main()
        {
            var container = new Container();

            // Register individual validators
            container.Register<IMissionValidator, TimeWindowValidator>();
            container.Register<IMissionValidator, AltitudeValidator>();
            container.Register<IMissionValidator, PowerValidator>();

            // Register the Facade with all validators automatically injected
            container.Register<MissionValidationFacade>();

            // Resolve the facade
            var missionFacade = container.Resolve<MissionValidationFacade>();

            // Define a sample mission
            var mission = new MissionPlan
            {
                StartTime = new DateTime(2025, 5, 10, 8, 0, 0),
                EndTime = new DateTime(2025, 5, 10, 21, 0, 0),
                Altitude = 600,
                PowerConsumption = 90
            };

            bool isValid = missionFacade.ValidateMission(mission);
            Console.WriteLine($"\nMission Validation Result: {(isValid ? "Approved" : "Rejected")}");
            Console.ReadLine();
        }
    }
}
