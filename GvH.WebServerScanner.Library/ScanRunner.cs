using GvH.WebServerScanner.Library.Entities;
using System.Net.NetworkInformation;
using System.Net;

namespace GvH.WebServerScanner.Library
{
    public class ScanRunner
    {
        public IpAddressScanResult PollIpAddress(IpAddressRepresentation ip, List<HttpScanParameter> scanParams)
        {
            var res = new IpAddressScanResult() { IpAddress = ip };
            res.PingMs = PingMs(ip);
            res.HostName = GetHostName(ip);
            res.HttpResults = scanParams.Select(s => PerformHttpRequest(ip, s)).ToList();
            return res;
        }

        private int? PingMs(IpAddressRepresentation ip)
        {

            var pingReq = new Ping();
            try
            {
                var reply = pingReq.Send(ip.GetRepresentation(), 500); //Ping IP address with 500ms timeout
                return (int)reply.RoundtripTime;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetHostName(IpAddressRepresentation ip)
        {
            try
            {
                var addr = IPAddress.Parse(ip.GetRepresentation());
                var host = Dns.GetHostEntry(addr);
                return host.HostName;
            }
            catch (Exception e)
            {
                return "?";
            }
        }

        private IpAddressHttpResult PerformHttpRequest(IpAddressRepresentation ip, HttpScanParameter parameter)
        {
            try
            {
                HttpClient cli = new HttpClient();
                string composedUrl = $"{(parameter.Https ? "https" : "http")}://{ip.GetRepresentation()}:{parameter.Port}/";
                Console.WriteLine("Running for:" + composedUrl);
                var res = cli.GetAsync(composedUrl).GetAwaiter().GetResult();
                var content = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var title = ExtractTitleFromHtml(content);
                return new IpAddressHttpResult() { Port = parameter.Port, Https = parameter.Https, FoundResult = true, BodyContent = content, Title = title };
            }
            catch (Exception e)
            {
                return new IpAddressHttpResult() { Port = parameter.Port, Https = parameter.Https, FoundResult = false, BodyContent = e.Message };
            }
        }

        private string ExtractTitleFromHtml(string html)
        {
            int idxS = html.IndexOf("<title>", StringComparison.OrdinalIgnoreCase);
            int idxE = html.IndexOf("</title>", StringComparison.OrdinalIgnoreCase);
            if (idxS > -1 && idxE > -1)
            {
                return html.Substring(idxS + 7, idxE - (idxS + 7)).Trim();
            }
            return "No Title";
        }
    }
}