﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace CallMeAPI
{
    public class Program
    {
        public static bool onAzure = false;
        public static string Host = "api.talktoleadsnow.com";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args).UseKestrel(options => {
            options.ListenAnyIP(5090); //HTTP port
        })
                .UseStartup<Startup>();
    }
}
