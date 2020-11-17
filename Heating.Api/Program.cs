using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Heating.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if (!DEBUG)
            Console.WriteLine("Release Version");
#endif

            new Heating.Core.HeatingManager();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
#if !DEBUG
                .UseKestrel(o =>
                {
                    o.ListenAnyIP(5000); // default http pipeline
                })
#endif
                .UseStartup<Startup>();
    }
}
