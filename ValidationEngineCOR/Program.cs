using System;

namespace ChainOfResponsibilityValidation
{
    // Step 1: Define the Base Validator Class (Handler)
    public abstract class MissionValidator
    {
        protected MissionValidator _nextValidator;

        public void SetNext(MissionValidator nextValidator)
        {
            _nextValidator = nextValidator;
        }

        public virtual bool Validate(MissionPlan mission)
        {
            if (_nextValidator != null)
            {
                return _nextValidator.Validate(mission);
            }
            return true; // No more validators, so return success
        }
    }

    // Step 2: Concrete Validators (Handlers)

    public class TimeWindowValidator : MissionValidator
    {
        public override bool Validate(MissionPlan mission)
        {
            if (mission.StartTime.Hour < 6 || mission.EndTime.Hour > 22)
            {
                Console.WriteLine("Time constraint failed.");
                return false;
            }
            Console.WriteLine("Time constraint passed.");

            return base.Validate(mission);
        }
    }

    public class AltitudeValidator : MissionValidator
    {
        public override bool Validate(MissionPlan mission)
        {
            if (mission.Altitude < 500 || mission.Altitude > 1000)
            {
                Console.WriteLine("Altitude constraint failed.");
                return false;
            }
            Console.WriteLine("Altitude constraint passed.");

            return base.Validate(mission);
        }
    }

    public class PowerValidator : MissionValidator
    {
        public override bool Validate(MissionPlan mission)
        {
            if (mission.PowerConsumption > 100)
            {
                Console.WriteLine("Power constraint failed.");
                return false;
            }
            Console.WriteLine("Power constraint passed.");

            return base.Validate(mission);
        }
    }

    // Step 3: Define the Mission Plan
    public class MissionPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Altitude { get; set; }
        public int PowerConsumption { get; set; }
    }

    // Step 4: Run the Example
    class Program
    {
        static void Main()
        {
            // Create validators
            MissionValidator timeValidator = new TimeWindowValidator();
            MissionValidator altitudeValidator = new AltitudeValidator();
            MissionValidator powerValidator = new PowerValidator();

            // Chain them together
            timeValidator.SetNext(altitudeValidator);
            altitudeValidator.SetNext(powerValidator);

            // Create a mission
            MissionPlan mission = new MissionPlan
            {
                StartTime = new DateTime(2025, 5, 10, 8, 0, 0),
                EndTime = new DateTime(2025, 5, 10, 21, 0, 0),
                Altitude = 600,
                PowerConsumption = 90
            };

            // Start the validation chain
            bool isValid = timeValidator.Validate(mission);

            Console.WriteLine($"\nMission Validation Result: {(isValid ? "Approved" : "Rejected")}");
            Console.ReadLine();
        }
    }
}
