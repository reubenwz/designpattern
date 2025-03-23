using System;

namespace VirtualProxyDemo
{
    // Step 1: Define Interface
    public interface ISatelliteImage
    {
        void Display();
    }

    // Step 2: Implement Real Image (Expensive to Create)
    public class HighResSatelliteImage : ISatelliteImage
    {
        private string _fileName;

        public HighResSatelliteImage(string fileName)
        {
            _fileName = fileName;
            LoadImage();
        }

        private void LoadImage()
        {
            Console.WriteLine($"Loading High-Resolution Image: {_fileName}");
        }

        public void Display()
        {
            Console.WriteLine($"Displaying Image: {_fileName}");
        }
    }

    // Step 3: Implement Virtual Proxy
    public class SatelliteImageProxy : ISatelliteImage
    {
        private HighResSatelliteImage _realImage;
        private string _fileName;

        public SatelliteImageProxy(string fileName)
        {
            _fileName = fileName;
        }

        public void Display()
        {
            if (_realImage == null)
            {
                _realImage = new HighResSatelliteImage(_fileName);
            }
            _realImage.Display();
        }
    }

    // Step 4: Main Program
    class Program
    {
        static void Main()
        {
            ISatelliteImage image = new SatelliteImageProxy("earth_map.png");

            Console.WriteLine("Image object created, but not loaded yet.");

            Console.WriteLine("\nNow displaying image:");
            image.Display(); // Loads and displays image

            Console.WriteLine("\nDisplaying image again:");
            image.Display(); // Uses cached object
        }
    }
}
