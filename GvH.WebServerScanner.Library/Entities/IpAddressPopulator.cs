using Microsoft.Extensions.Logging;

namespace GvH.WebServerScanner.Library.Entities
{
    public class IpAddressPopulator
    {
        private readonly ILogger<IpAddressPopulator> logger;

        public IpAddressPopulator(ILogger<IpAddressPopulator> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<IpAddressRepresentation> PopulateRange(IpAddressRepresentation from, IpAddressRepresentation to)
        {
            List<IpAddressRepresentation> res = new List<IpAddressRepresentation>();
            var curr = from;
            
            // Add from
            res.Add(new IpAddressRepresentation(curr));

            while (curr.GetRepresentation() != to.GetRepresentation())
            {
                curr = curr.AddStep(1);
                logger.LogDebug($"Found: {curr.GetRepresentation()}");
                res.Add(new IpAddressRepresentation(curr)); // With this step the last is always added too!

                if(res.Count > 10000) { throw new OverflowException("System did not end before threshold, possible error!"); }
            }
            return res;
        }
    }
}