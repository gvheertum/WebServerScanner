using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Threading.Tasks;
using GvH.WebServerScanner.Library.Entities;

namespace GvH.WebServerScanner.Library
{
    public class DependencyInjection
    {
        private static ServiceCollection ServiceCollection;

        public ServiceCollection ConfigureDependencyInjection()
        {
            ServiceCollection = new ServiceCollection();

            // Configuration
            var configuration = GetConfiguration();

            //Services
            ServiceCollection.AddSingleton<IpAddressPopulator>();
            ServiceCollection.AddSingleton<ScanRunner>();


            // Logging
            var nlogConfiguration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
            ServiceCollection.AddLogging(configure => configure.AddNLog(nlogConfiguration));
            ServiceCollection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Trace);

            return ServiceCollection;
        }

        public static void AddTransient<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            ServiceCollection.AddTransient<TInterface, TType>();
        }


        public static IServiceProvider CreateServiceProvider()
        {
            return ServiceCollection.BuildServiceProvider();
        }

        public static IConfigurationRoot GetConfiguration()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
               .AddJsonFile("appsettings.secret.json", optional: true, reloadOnChange: true)
               .Build();
            return configuration;
        }
    }
}
