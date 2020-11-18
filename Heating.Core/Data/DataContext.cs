using Microsoft.EntityFrameworkCore;
using Heating.PubSub;
using System;
using System.Collections.Generic;
using System.Text;
using Heating.Core;
using System.IO;
using Heating.Core.Schedule;
using Microsoft.Extensions.Configuration;

namespace Heating.Core.Data
{
    /// <summary>
    /// Enable EntityFramework datatable access for the application:
    /// To update the database structure:
    /// - In command prompt, navigate to the application root (eg. C:\\ ... \Heating.Core\)
    /// - Enter command: dotnet ef migrations add InitialCreate
    /// - Enter command: dotnet ef database update
    /// This will build the database in the Project root directory
    /// In theory one would then set the property of the .db file to Copy when newer, to copy it to the output file,
    /// but for some reason the executing project looks for the database file in its own root directory. This does get created at runtime, but without
    /// any tables, so the created datatable has o be manually copied to the executing project's root directory when it was updated.
    /// </summary>
    public class DataContext : DbContext
    {
        public DbSet<Relay> Relays { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<ScheduledEvent> ScheduledEvents { get; set; }
        public DbSet<InfoLog> InfoLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Settings.Configuration.GetConnectionString("hhdbstring"));            
        }
    }
}
