﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;



namespace ERPMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var bindingConfig = new ConfigurationBuilder()
             .AddCommandLine(args)
             .Build();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   })
                .UseNLog()
                 //.UseKestrel(options =>
                 //{
                 //    options.Limits.MaxRequestHeadersTotalSize = 1048576;
                 //}) 
                 // TODO: Cambiaar el funcionamiento de los permisos , solucion temporal de incrementar el valor de la variable                 
                 .ConfigureKestrel((context, options) =>
                 {
                     options.Limits.MaxRequestHeadersTotalSize = 1048576;
                 })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}
