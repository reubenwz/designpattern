using System;
using System.Collections.Generic;

namespace PropertyProxyDemo
{
    // Simulated TLE Validator
    public class TleValidator
    {
        public static bool IsValid(string tle)
        {
            return !string.IsNullOrWhiteSpace(tle) && tle.Length > 50; // Simplified validation
        }
    }

    // Proxy for controlling access to the TLE property
    public class TleProxy
    {
        private string _tle;
        private readonly string _authorizedUser;
        private string _cachedOrbit; // Caching computed orbit
        private bool _isDirty = true; // Indicates if recomputation is needed

        public TleProxy(string initialTle, string authorizedUser)
        {
            if (!TleValidator.IsValid(initialTle))
                throw new ArgumentException("Invalid TLE format.");

            _tle = initialTle;
            _authorizedUser = authorizedUser;
        }

        public string TLE
        {
            get => GetTle();
            set => SetTle(value);
        }

        private string GetTle()
        {
            Console.WriteLine($"[LOG] Accessing TLE value.");
            return _tle;
        }

        private void SetTle(string newTle)
        {
            Console.WriteLine($"[LOG] Attempting to update TLE...");

            if (!TleValidator.IsValid(newTle))
            {
                Console.WriteLine($"[ERROR] Invalid TLE format. Update rejected.");
                return;
            }

            if (_authorizedUser != "admin")
            {
                Console.WriteLine($"[SECURITY] Unauthorized TLE update attempt by {_authorizedUser}.");
                return;
            }

            _tle = newTle;
            _isDirty = true; // Mark as needing recomputation
            Console.WriteLine($"[SUCCESS] TLE updated.");
        }

        public string GetPropagatedOrbit()
        {
            if (!_isDirty)
            {
                Console.WriteLine($"[CACHE] Returning cached orbit result.");
                return _cachedOrbit;
            }

            Console.WriteLine($"[PROCESS] Computing orbit propagation...");
            _cachedOrbit = $"Propagated Orbit Data for {_tle.Substring(0, 10)}...";
            _isDirty = false; // Mark as up to date

            return _cachedOrbit;
        }
    }

    // Spacecraft class that uses TleProxy
    public class Spacecraft
    {
        public string Name { get; }
        private readonly TleProxy _tleProxy;

        public Spacecraft(string name, string tle, string user)
        {
            Name = name;
            _tleProxy = new TleProxy(tle, user);
        }

        public void ShowTle()
        {
            Console.WriteLine($"[INFO] {Name} TLE: {_tleProxy.TLE}");
        }

        public void UpdateTle(string newTle)
        {
            _tleProxy.TLE = newTle;
        }

        public void ComputeOrbit()
        {
            Console.WriteLine($"[INFO] {Name} Orbit: {_tleProxy.GetPropagatedOrbit()}");
        }
    }

    // Main Program
    class Program
    {
        static void Main()
        {
            Console.WriteLine("🚀 Spacecraft Property Proxy Demo\n");

            // Create spacecraft with initial TLE and authorized user
            var spacecraft = new Spacecraft("TeLEOS-2", "1 25544U 98067A   24058.54835648  .00016717 433334443", "admin");

            // Validate and access TLE
            spacecraft.ShowTle();

            // Compute orbit for the first time (triggers processing)
            spacecraft.ComputeOrbit();

            // Compute orbit again (should return cached result)
            spacecraft.ComputeOrbit();

            // Unauthorized TLE update attempt
            spacecraft.UpdateTle("1 12345X INVALID TLE FORMAT");

            // Authorized TLE update attempt
            spacecraft.UpdateTle("1 25544U 98067A   24060.12345678  .00023456");

            // Recompute orbit after valid TLE update
            spacecraft.ComputeOrbit();

            Console.ReadLine();
        }
    }
}
