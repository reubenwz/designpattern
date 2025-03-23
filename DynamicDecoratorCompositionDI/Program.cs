using System;
using System.Collections.Generic;
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

    public class CompositeMissionValidator : IMissionValidator
    {
        private readonly IEnumerable<IMissionValidator> _validators;

        public CompositeMissionValidator(IEnumerable<IMissionValidator> validators)
        {
            _validators = validators;
        }

        public bool Validate(MissionPlan mission)
        {
            foreach (var validator in _validators)
            {
                if (!validator.Validate(mission))
                {
                    return false;
                }
            }
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

            container.Register<IMissionValidator, BasicMissionValidator>();
            container.Register<IMissionValidator, TimeWindowValidator>();
            container.Register<IMissionValidator, AltitudeValidator>();
            container.Register<IMissionValidator, PowerValidator>();

            container.Register<IMissionValidator, CompositeMissionValidator>();

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
