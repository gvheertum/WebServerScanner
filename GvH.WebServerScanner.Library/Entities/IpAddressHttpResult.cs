namespace GvH.WebServerScanner.Library.Entities
{
    public class IpAddressHttpResult
    {
        public int Port { get; set; }
        public bool Https { get; set; }
        public bool FoundResult { get; set; }
        public string Title { get; set; }
        public string BodyContent { get; set; }
    }
}