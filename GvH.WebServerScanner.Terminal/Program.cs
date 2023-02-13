using GvH.WebServerScanner.Library;
using GvH.WebServerScanner.Library.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GvH.WebServerScanner.Terminal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
              .ConfigureServices(c =>
              {
                  new DependencyInjection().ConfigureDependencyInjection(c);
              })
              .Build();

            Console.WriteLine("IP Scan");
            Console.WriteLine("Usage: GvH.WebServerScanner.Terminal.exe {StartIp} {EndIp} {port1},{port2}");

            string ipStart = args.Length >= 1 ? args[0] : "";
            string ipEnd = args.Length >= 2 ? args[1] : "";
            var ports = DeterminePorts(args.Length >= 3 ? args[2] : "");

            if(string.IsNullOrEmpty(ipStart) || string.IsNullOrEmpty(ipEnd) || !ports.Any()) {
                Console.WriteLine("INVALID PARAMS");
                return;
            }

            //TODO: Load a range from the params
            //TODO: DI
            //TODO: Logging
            var ipAddressRange = host.Services.GetRequiredService<IpAddressPopulator>().PopulateRange(new IpAddressRepresentation(ipStart), new IpAddressRepresentation(ipEnd));
            Console.WriteLine($"Scanning {ipAddressRange.Count()} ip addresses");
            var results = host.Services.GetRequiredService<ScanRunner>().ScanAddresses(ipAddressRange, ports, EchoResult);

            foreach (var res in results)
            {
                EchoResult(res);              
            }
        }

        static IEnumerable<HttpScanParameter> DeterminePorts(string input)
        {
            var ports = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(i => { return (int?)(Int32.TryParse(i, out int x) ? x : null); });
            return ports.Where(p => p != null).Select(p => new HttpScanParameter() { Port = p.Value }).ToList();
        }

        static object writeBlock = new object();

        static void EchoResult(IpAddressScanResult res)
        {
            lock(writeBlock) 
            { //Try to keep the data together
                if (res.Pingable)
                {
                    Console.WriteLine($"-----------------------------------------");
                    Console.WriteLine($"{res.IpAddress.GetRepresentation()} (hostname: {res.HostName}): Online -> {res.Pingable} - {res.PingMs}ms");
                    foreach (var x in res.HttpResults)
                    {
                        if (x.FoundResult)
                        {
                            Console.WriteLine($"{res.IpAddress.GetRepresentation()}:{x.Port}: {x.Title}");
                        }
                    }
                }
            } //Ignore non pingables
        }
    }
}