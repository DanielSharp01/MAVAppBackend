using MAVAppBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        { }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<TrainInstance> TrainInstances { get; set; }
        public DbSet<TrainStation> TrainStations { get; set; }
        public DbSet<TrainStationLink> TrainStationLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>().Property(e => e.Name).IsRequired(true).HasMaxLength(255);
            modelBuilder.Entity<TrainInstance>().Property(e => e.ElviraId).IsRequired(true).HasMaxLength(16);
            modelBuilder.Entity<TrainStationLink>().Property(e => e.FromId).IsRequired(false);
            modelBuilder.Entity<TrainStationLink>().Property(e => e.ToId).IsRequired(false);
        }
    }
}
