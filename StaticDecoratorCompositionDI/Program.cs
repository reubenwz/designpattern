using System;
using DryIoc;

namespace ValidationEngineDecorator
{
    public interface IMissionValidator
    {
        bool Validate(MissionPlan mission);
    }

    public class BasicMissionValidator : IMissionValidator
    {
        public bool Validate(MissionPlan mission)
        {
            Console.WriteLine("Basic validation passed.");
            return true;
        }
    }

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

    public class TimeWindowValidator : MissionValidatorDecorator
    {
        public TimeWindowValidator(IMissionValidator validator) : base(validator) { }

        public override bool Validate(MissionPlan mission)
        {
            if (!base.Validate(mission)) return false;

            if (mission.StartTime.Hour < 6 || mission.EndTime.Hour > 22)
            {
                Console.WriteLine("Mission rejected: Outside allowed time window.");
                return false;
            }
            Console.WriteLine("Time Window Constraint passed.");
            return true;
        }
    }

    public class AltitudeValidator : MissionValidatorDecorator
    {
        public AltitudeValidator(IMissionValidator validator) : base(validator) { }

        public override bool Validate(MissionPlan mission)
        {
            if (!base.Validate(mission)) return false;

            if (mission.Altitude < 500 || mission.Altitude > 1000)
            {
                Console.WriteLine("Mission rejected: Altitude out of safe range.");
                return false;
            }
            Console.WriteLine("Altitude Constraint passed.");
            return true;
        }
    }

    public class PowerValidator : MissionValidatorDecorator
    {
        public PowerValidator(IMissionValidator validator) : base(validator) { }

        public override bool Validate(MissionPlan mission)
        {
            if (!base.Validate(mission)) return false;

            if (mission.PowerConsumption > 100)
            {
                Console.WriteLine("Mission rejected: Power consumption too high.");
                return false;
            }
            Console.WriteLine("Power Constraint passed.");
            return true;
        }
    }

    public class MissionPlan
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Altitude { get; set; }
        public int PowerConsumption { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var container = new Container();

            // Register base validator
            container.Register<IMissionValidator, BasicMissionValidator>();

            // Register decorators in the correct order
            container.Register<IMissionValidator, TimeWindowValidator>(setup: Setup.Decorator);
            container.Register<IMissionValidator, AltitudeValidator>(setup: Setup.Decorator);
            container.Register<IMissionValidator, PowerValidator>(setup: Setup.Decorator);

            var mission = new MissionPlan
            {
                StartTime = new DateTime(2025, 5, 10, 8, 0, 0),
                EndTime = new DateTime(2025, 5, 10, 21, 0, 0),
                Altitude = 600,
                PowerConsumption = 90
            };

            Console.WriteLine("Validating Mission Plan...\n");

            var validator = container.Resolve<IMissionValidator>();

            bool isValid = validator.Validate(mission);

            Console.WriteLine($"\nMission Validation Result: {(isValid ? "Approved" : "Rejected")}");
            Console.ReadLine();
        }
    }
}
