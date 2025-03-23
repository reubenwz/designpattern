using System;

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
            Console.WriteLine("✔️ Basic validation passed.");
            return true; // Base validation always passes
        }
    }

    // Step 3: Create the Decorator Base Class
    public abstract class MissionValidatorDecorator : IMissionValidator
    {
        protected IMissionValidator _validator;

        public MissionValidatorDecorator(IMissionValidator validator)
        {
            _validator = validator;
        }

        public virtual bool Validate(MissionPlan mission)
        {
            return _validator.Validate(mission);
        }
    }

    // Step 4: Create Concrete Decorators (Constraints)

    // 🔹 Time Window Constraint
    public class TimeWindowValidator : MissionValidatorDecorator
    {
        public TimeWindowValidator(IMissionValidator validator) : base(validator) { }

        public override bool Validate(MissionPlan mission)
        {
            if (!base.Validate(mission)) return false;

            if (mission.StartTime.Hour < 6 || mission.EndTime.Hour > 22)
            {
                Console.WriteLine("❌ Mission rejected: Outside allowed time window.");
                return false;
            }

            Console.WriteLine("✔️ Time Window Constraint passed.");
            return true;
        }
    }

    // 🔹 Altitude Constraint
    public class AltitudeValidator : MissionValidatorDecorator
    {
        public AltitudeValidator(IMissionValidator validator) : base(validator) { }

        public override bool Validate(MissionPlan mission)
        {
            if (!base.Validate(mission)) return false;

            if (mission.Altitude < 500 || mission.Altitude > 1000)
            {
                Console.WriteLine("❌ Mission rejected: Altitude out of safe range.");
                return false;
            }

            Console.WriteLine("✔️ Altitude Constraint passed.");
            return true;
        }
    }

    // 🔹 Power Constraint
    public class PowerValidator : MissionValidatorDecorator
    {
        public PowerValidator(IMissionValidator validator) : base(validator) { }

        public override bool Validate(MissionPlan mission)
        {
            if (!base.Validate(mission)) return false;

            if (mission.PowerConsumption > 100)
            {
                Console.WriteLine("❌ Mission rejected: Power consumption too high.");
                return false;
            }

            Console.WriteLine("✔️ Power Constraint passed.");
            return true;
        }
    }

    // Step 5: Define the MissionPlan Class
    public class MissionPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Altitude { get; set; } // In km
        public int PowerConsumption { get; set; } // In arbitrary units
    }

    // Step 6: Main Program to Run Validation
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

            Console.WriteLine("🚀 Validating Mission Plan...\n");

            // Base validator
            IMissionValidator validator = new BasicMissionValidator();

            // Apply decorators dynamically
            validator = new TimeWindowValidator(validator);
            validator = new AltitudeValidator(validator);
            validator = new PowerValidator(validator);

            // Run validation
            bool isValid = validator.Validate(mission);

            Console.WriteLine($"\n✅ Mission Validation Result: {(isValid ? "Approved" : "Rejected")}");
            Console.ReadLine(); // Keep console open
        }
    }
}
