namespace GvH.WebServerScanner.Library.Entities
{
    public class IpAddressScanResult
    {
        public IpAddressRepresentation IpAddress { get; set; }
        public bool Pingable { get { return PingMs != null; } }
        public int? PingMs { get; set; }
        public string HostName { get; set; }
        public IEnumerable<IpAddressHttpResult> HttpResults { get; set; }
    }
}