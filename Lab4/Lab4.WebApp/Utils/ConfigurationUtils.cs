using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.WebApp
{
    public static class ConfigurationUtils
    {
        public const string appSettingsFile = "appsettings.json";

        private static IConfigurationRoot _configurationBuilder;

        public static IConfigurationRoot ConfigurationRoot =>
            _configurationBuilder ?? (
                _configurationBuilder =
                new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile(appSettingsFile)
                .Build());

        public static string DefaultConnectionString =>
            ConfigurationRoot.GetConnectionString("SocialNet");

        public static string FileStoragePath =>
            ConfigurationRoot.GetValue<string>("FileStoragePath");

    }
}
