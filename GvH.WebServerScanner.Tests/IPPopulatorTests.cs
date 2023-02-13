using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using System.Net;
using GvH.WebServerScanner.Library.Entities;
using GvH.WebServerScanner.Library;
using System.Security.Cryptography;

namespace GvH.WebServerScanner.Tests
{
    public class IPPopulatorTests
    {
        private IpAddressPopulator ipAddressPopulator;

        [SetUp]
        public void Setup()
        {
            var host = new HostBuilder()
              .ConfigureServices(c =>
              {
                  new DependencyInjection().ConfigureDependencyInjection(c);
              })
              .Build();

            ipAddressPopulator = host.Services.GetRequiredService<IpAddressPopulator>();

        }

        [Test]
        public void IPPopulator_RetrieveSingle_ReturnsSingleIp()
        {
            // Arrange

            // Act
            var range = ipAddressPopulator.PopulateRange(new IpAddressRepresentation("192.168.1.1"), new IpAddressRepresentation("192.168.1.1"));

            // Assert
            range.Should().HaveCount(1);            
        }

        [Test]
        public void IPPopulator_RetrieveOverBoundary_CorrectlyReturnsNextSegmentIncrease()
        {
            // Act
            var range = ipAddressPopulator.PopulateRange(new IpAddressRepresentation("192.168.1.255"), new IpAddressRepresentation("192.168.2.0"));

            // Assert
            range.Should().HaveCount(2);
            //TODO: More
        }


        [Test]
        public void IPPopulator_Retrieve_YieldsCorrectData()
        {
            // Act
            var range = ipAddressPopulator.PopulateRange(new IpAddressRepresentation("192.168.1.1"), new IpAddressRepresentation("192.168.1.50"));

            // Assert
            range.Should().HaveCount(50);
            //TODO: More
        }

        [Test]
        //Case: Invalid IP range
        //Case: Reversed IP range
        public void IPPopulator_InvalidInput_YieldsNoResults()
        {
            Assert.Fail("Not build yet");
        }
    }
}

//TODO: Create test class for IP add/substract stuff 