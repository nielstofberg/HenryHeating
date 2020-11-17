using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heating.Core
{
    public static class Settings
    {
        public static IConfiguration Configuration { get; set; }

        static Settings()
        {
            Configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();
        }
    }
}
