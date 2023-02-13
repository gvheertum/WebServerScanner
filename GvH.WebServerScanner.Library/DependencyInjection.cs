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
        private static IServiceCollection _serviceCollection;

        public IServiceCollection ConfigureDependencyInjection(IServiceCollection collection)
        {
            collection = collection ?? new ServiceCollection();
            _serviceCollection = collection;

            // Configuration
            var configuration = GetConfiguration();

            //Services
            _serviceCollection.AddSingleton<IpAddressPopulator>();
            _serviceCollection.AddSingleton<ScanRunner>();
            _serviceCollection.AddSingleton<IpScanRun>();


            // Logging
            var nlogConfiguration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
            _serviceCollection.AddLogging(configure => configure.AddNLog(nlogConfiguration));
            _serviceCollection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Trace);

            return _serviceCollection;
        }

        public static void AddTransient<TInterface, TType>()
            where TInterface : class
            where TType : class, TInterface
        {
            _serviceCollection.AddTransient<TInterface, TType>();
        }


        public static IServiceProvider CreateServiceProvider()
        {
            return _serviceCollection.BuildServiceProvider();
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
