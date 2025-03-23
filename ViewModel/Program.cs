using System;
using System.ComponentModel;

namespace MVVMProxyExample
{
    // Step 1: Model (Business Logic)
    public class SatelliteMission
    {
        public string MissionName { get; set; }
        public int Altitude { get; set; } // In km
    }

    // Step 2: ViewModel (Proxy between View and Model)
    public class SatelliteMissionViewModel : INotifyPropertyChanged
    {
        private readonly SatelliteMission _mission;
        public event PropertyChangedEventHandler PropertyChanged;

        public SatelliteMissionViewModel(SatelliteMission mission)
        {
            _mission = mission;
        }

        public string MissionName
        {
            get { return _mission.MissionName; }
            set
            {
                _mission.MissionName = value;
                NotifyPropertyChanged(nameof(MissionName));
            }
        }

        public string Altitude // Proxy provides a formatted altitude
        {
            get { return $"{_mission.Altitude} km"; }
            set
            {
                int parsedAltitude;
                if (int.TryParse(value.Replace(" km", ""), out parsedAltitude) && parsedAltitude >= 500)
                {
                    _mission.Altitude = parsedAltitude;
                    NotifyPropertyChanged(nameof(Altitude));
                }
                else
                {
                    Console.WriteLine("Invalid altitude: Must be at least 500 km.");
                }
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Step 3: Simulating the View (Console UI)
    class Program
    {
        static void Main()
        {
            SatelliteMission mission = new SatelliteMission { MissionName = "Observation-1", Altitude = 600 };
            SatelliteMissionViewModel viewModel = new SatelliteMissionViewModel(mission);

            Console.WriteLine($"Initial Mission Name: {viewModel.MissionName}");
            Console.WriteLine($"Initial Altitude: {viewModel.Altitude}");

            // Updating through ViewModel (acts as proxy)
            viewModel.MissionName = "Mars Exploration";
            viewModel.Altitude = "450 km"; // Invalid case
            viewModel.Altitude = "700 km"; // Valid case

            Console.WriteLine($"Updated Mission Name: {viewModel.MissionName}");
            Console.WriteLine($"Updated Altitude: {viewModel.Altitude}");

            Console.ReadLine();
        }
    }
}
