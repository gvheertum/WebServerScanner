namespace GvH.WebServerScanner.Library.Entities
{
    public class IpAddressRepresentation
    {
        public IpAddressRepresentation(int p1, int p2, int p3, int p4)
        {
            Part1 = p1;
            Part2 = p2;
            Part3 = p3;
            Part4 = p4;
        }

        public IpAddressRepresentation(string ip)
        {
            //TODO: Hardening
            string[] startIPString = ip.Split('.');
            int[] startIP = Array.ConvertAll(startIPString, int.Parse);
            Part1 = startIP.Length >= 1 ? startIP[0] : 0;
            Part2 = startIP.Length >= 2 ? startIP[1] : 0;
            Part3 = startIP.Length >= 3 ? startIP[2] : 0;
            Part4 = startIP.Length >= 4 ? startIP[3] : 0;
        }

        public int Part1 { get; set; }
        public int Part2 { get; set; }
        public int Part3 { get; set; }
        public int Part4 { get; set; }

        public string GetRepresentation()
        {
            return $"{Part1}.{Part2}.{Part3}.{Part4}";
        }

        public IpAddressRepresentation AddStep(int amount)
        {
            return this;

        }

        private bool Overflows(int segment, int amountToIncrease)
        {
            return false;
        }

        public bool IsValid()
        {
            return
                Part1 > 0 && Part1 < 255 && // 0 and 255 not valid for first part
                Part2 >= 0 && Part2 <= 255 &&
                Part3 >= 0 && Part3 <= 255 &&
                Part4 >= 0 && Part4 <= 255;
        }
    }
}