using GvH.WebServerScanner.Library.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace GvH.WebServerScanner.Library
{
    public class ScanRunner
    {
        private readonly ILogger<ScanRunner> logger;
        private readonly IpScanRun ipScanRun;

        public ScanRunner(ILogger<ScanRunner> logger, IpScanRun ipScanRun)
        {
            this.logger = logger;
            this.ipScanRun = ipScanRun;
        }


        public IEnumerable<IpAddressScanResult> ScanAddresses(IEnumerable<IpAddressRepresentation> ipAddresses, IEnumerable<HttpScanParameter> scanParams, Action<IpAddressScanResult> retrieveAction)
        {
            logger.LogDebug($"Scanning: {ipAddresses.Count()} ip's on {scanParams.Count()} ports: Callback? {(retrieveAction == null ? "NO" : "YES")}");
            ConcurrentBag<IpAddressScanResult> data = new ConcurrentBag<IpAddressScanResult>();
            Parallel.ForEach(ipAddresses, address =>
            {
                var res = ipScanRun.GetInstance().PollIpAddress(address, scanParams);
                if(retrieveAction != null) { retrieveAction(res); }
                data.Add(res);
            });

            return data.ToList();
        }
    }
}