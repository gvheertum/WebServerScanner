using GvH.WebServerScanner.Library;
using GvH.WebServerScanner.Library.Entities;

namespace GvH.WebServerScanner.Terminal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var res = new ScanRunner().PollIpAddress(new IpAddressRepresentation("192.168.1.1"), new List<HttpScanParameter>() { new HttpScanParameter() { Port = 80 } });
            
            Console.WriteLine($"{res.IpAddress.GetRepresentation()} ({res.HostName}): Online -> {res.Pingable} - {res.PingMs}ms");
            foreach(var x in res.HttpResults)
            {
                Console.WriteLine($"Connected {x.FoundResult}: {x.Title}");
            }
        }
    }
}