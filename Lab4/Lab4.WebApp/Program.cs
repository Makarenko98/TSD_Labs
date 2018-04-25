using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lab4.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            EnsureFileStorageCreated();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static void EnsureFileStorageCreated()
        {
            var storagePath = ConfigurationUtils.FileStoragePath;
            if (!Directory.Exists(storagePath)) {
                Directory.CreateDirectory(storagePath);
            }
        }
    }
}
