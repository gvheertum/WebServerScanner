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

        [TestCase("192.168.123.10", "192.168.123.1")] // Incorrect order
        [TestCase("999.999.999.10", "999.999.999.12")] // Incorrect order
        [TestCase("aaa", "192.168.123.1")] // Invalid
        [TestCase("192.168.123.1", "a.a.a.a")] // Invalid
        [TestCase("", "")] // emtpy
        [TestCase(null, null)] // Empty
        [TestCase(null, "192.168.123.10")] // Empty
        [TestCase("192.168.123.10", null)] // Empty
        public void IPPopulator_InvalidInput_YieldsNoResults(string from, string until)
        {
            Assert.Throws<FormatException>(() =>
            {
                // Act
                var range = ipAddressPopulator.PopulateRange(new IpAddressRepresentation(from), new IpAddressRepresentation(until));
            });
        }

        [TestCase("1.1.1.2", "1.1.1.1", true)]
        [TestCase("1.1.2.2", "1.1.1.1", true)]
        [TestCase("1.2.2.2", "1.1.1.1", true)]
        [TestCase("2.2.2.2", "1.1.1.1", true)]
        [TestCase("2.1.1.1", "1.1.1.1", true)]
        [TestCase("1.1.2.1", "1.1.1.1", true)]
        [TestCase("1.1.2.0", "1.1.1.1", true)]
        [TestCase("1.1.2.0", "1.1.1.255", true)]
        [TestCase("1.2.0.0", "1.1.255.0", true)]
        [TestCase("2.0.0.0", "1.255.0.0", true)]
        [TestCase("1.2.1.1", "1.1.1.1", true)]
        [TestCase("2.1.1.1", "1.1.1.1", true)]
        [TestCase("1.1.1.1", "1.1.1.2", false)] // Reversed
        [TestCase("1.1.1.1", "1.1.2.2", false)]
        [TestCase("1.1.1.1", "1.2.2.2", false)]
        [TestCase("1.1.1.1", "2.2.2.2", false)]
        [TestCase("1.1.1.1", "2.1.1.1", false)]
        [TestCase("1.1.1.1", "1.1.2.1", false)]
        [TestCase("1.1.1.1", "1.2.1.1", false)]
        [TestCase("1.1.1.1", "2.1.1.1", false)]
        [TestCase("1.1.1.1", "1.1.1.1", false)] //Equal should NOT match
        public void IpAddress_GreaterThanComparison_YieldsCorrectResult(string addr1, string addr2, bool expected)
        {
            // Arrange
            var i1 = new IpAddressRepresentation(addr1);
            var i2 = new IpAddressRepresentation(addr2);

            i1.IsGreaterThan(i2).Should().Be(expected);
        }

        [TestCase("1.1.1.2", "1.1.1.1", true)]
        [TestCase("1.1.2.2", "1.1.1.1", true)]
        [TestCase("1.2.2.2", "1.1.1.1", true)]
        [TestCase("2.2.2.2", "1.1.1.1", true)]
        [TestCase("2.1.1.1", "1.1.1.1", true)]
        [TestCase("1.1.2.1", "1.1.1.1", true)]
        [TestCase("1.2.1.1", "1.1.1.1", true)]
        [TestCase("2.1.1.1", "1.1.1.1", true)]
        [TestCase("1.1.1.1", "1.1.1.2", false)] // Reversed
        [TestCase("1.1.1.1", "1.1.2.2", false)]
        [TestCase("1.1.1.1", "1.2.2.2", false)]
        [TestCase("1.1.1.1", "2.2.2.2", false)]
        [TestCase("1.1.1.1", "2.1.1.1", false)]
        [TestCase("1.1.1.1", "1.1.2.1", false)]
        [TestCase("1.1.1.1", "1.2.1.1", false)]
        [TestCase("1.1.1.1", "2.1.1.1", false)]
        [TestCase("1.1.1.1", "1.1.1.1", true)] //Equal should match here
        public void IpAddress_GreaterThanOrEqualComparison_YieldsCorrectResult(string addr1, string addr2, bool expected)
        {
            // Arrange
            var i1 = new IpAddressRepresentation(addr1);
            var i2 = new IpAddressRepresentation(addr2);

            i1.IsGreaterThanOrEqual(i2).Should().Be(expected);
        }
    }
}

//TODO: Create test class for IP add/substract stuff 