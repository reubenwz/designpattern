using System;

namespace MethodChainingExample
{
    public class MissionPlan
    {
        public string Name { get; private set; }
        public int Altitude { get; private set; }
        public int Duration { get; private set; }
        public bool RequiresPowerBoost { get; private set; }

        public MissionPlan SetName(string name)
        {
            Name = name;
            return this; // Returning the same instance for chaining
        }

        public MissionPlan SetAltitude(int altitude)
        {
            Altitude = altitude;
            return this;
        }

        public MissionPlan SetDuration(int duration)
        {
            Duration = duration;
            return this;
        }

        public MissionPlan EnablePowerBoost()
        {
            RequiresPowerBoost = true;
            return this;
        }

        public void Execute()
        {
            Console.WriteLine($"Executing Mission: {Name}");
            Console.WriteLine($"Altitude: {Altitude} km");
            Console.WriteLine($"Duration: {Duration} minutes");
            Console.WriteLine($"Power Boost: {(RequiresPowerBoost ? "Enabled" : "Disabled")}");
        }
    }

    class Program
    {
        static void Main()
        {
            MissionPlan mission = new MissionPlan()
                .SetName("Observation-1")
                .SetAltitude(550)
                .SetDuration(120)
                .EnablePowerBoost();

            mission.Execute();
        }
    }
}
