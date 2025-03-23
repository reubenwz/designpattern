using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RemoteProxyExample
{
    // Step 1: Define an interface for fetching data
    public interface IRemoteService
    {
        string GetData();
    }

    // Step 2: Implement the remote proxy that fetches data from an external API
    public class RemoteServiceProxy : IRemoteService
    {
        private readonly string _apiUrl;

        public RemoteServiceProxy(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public string GetData()
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine("Fetching data from remote service...");
                Task<string> response = client.GetStringAsync(_apiUrl);
                response.Wait(); // Blocking call to support .NET 4.6
                return response.Result;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IRemoteService remoteService = new RemoteServiceProxy("https://jsonplaceholder.typicode.com/posts/1");

            string result = remoteService.GetData();
            Console.WriteLine("Response: " + result);

            Console.ReadLine();
        }
    }
}
