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

            if (!IsValid()) { throw new FormatException($"{Part1}.{Part2}.{Part3}.{Part4} is not a valid IP address"); }
        }

        public IpAddressRepresentation(IpAddressRepresentation input)
        {
            Part1 = input.Part1;
            Part2 = input.Part2;
            Part3 = input.Part3;
            Part4 = input.Part4;

            if (!IsValid()) { throw new FormatException($"{Part1}.{Part2}.{Part3}.{Part4} is not a valid IP address"); }
        }

        public IpAddressRepresentation(string ip)
        {
            if(string.IsNullOrWhiteSpace(ip)) { throw new FormatException($"{ip} is not a valid IP address"); }
            
            string[] startIPString = ip.Split('.');
            int[] startIP = Array.ConvertAll(startIPString, int.Parse);
            Part1 = startIP.Length >= 1 ? startIP[0] : 0;
            Part2 = startIP.Length >= 2 ? startIP[1] : 0;
            Part3 = startIP.Length >= 3 ? startIP[2] : 0;
            Part4 = startIP.Length >= 4 ? startIP[3] : 0;

            if(!IsValid()) { throw new FormatException($"{ip} is not a valid IP address"); }
        }

        public int Part1 { get; private set; }
        public int Part2 { get; private set; }
        public int Part3 { get; private set; }
        public int Part4 { get; private set; }

        public string GetRepresentation()
        {
            return $"{Part1}.{Part2}.{Part3}.{Part4}";
        }

        public IpAddressRepresentation AddStep(int amount)
        {
            if(amount > 255) { throw new Exception("Not going above 255 for steps"); }
            var newAddr = new IpAddressRepresentation(Part1, Part2, Part3, Part4);
            newAddr.Part4 += amount;
           
            if(newAddr.Part4 > 255) 
            {
                newAddr.Part3 += 1;
                newAddr.Part4 = newAddr.Part4 - 256; 
            }

            if (newAddr.Part3 > 255)
            {
                newAddr.Part2 += 1;
                newAddr.Part3 = newAddr.Part3 - 256;
            }

            if (newAddr.Part2 > 255)
            {
                newAddr.Part1 += 1;
                newAddr.Part2 = newAddr.Part2 - 256;
            }

            return newAddr;
        }

        public bool IsValid()
        {
            return
                Part1 > 0 && Part1 < 255 && // 0 and 255 not valid for first part
                Part2 >= 0 && Part2 <= 255 &&
                Part3 >= 0 && Part3 <= 255 &&
                Part4 >= 0 && Part4 <= 255;
        }


        public bool IsGreaterThanOrEqual(IpAddressRepresentation other)
        {
            if(other.Part1 == Part1 && other.Part2 == Part2 && other.Part3 == Part3 && other.Part4 == Part4) { return true; }
            return IsGreaterThan(other);
        }

        public bool IsGreaterThan(IpAddressRepresentation other)
        {
            if (Part1 > other.Part1) { return true; } // First node is higher
            if (Part1 == other.Part1 && Part2 > other.Part2) { return true; } // Second node higher
            if (Part1 == other.Part1 && Part2 == other.Part2 && Part3 > other.Part3) { return true; } //Third higher
            if (Part1 == other.Part1 && Part2 == other.Part2 && Part3 == other.Part3 && Part4 > other.Part4) { return true; } // Fourth higher

            // Others are lower, so false
            return false;
        }
    }
}