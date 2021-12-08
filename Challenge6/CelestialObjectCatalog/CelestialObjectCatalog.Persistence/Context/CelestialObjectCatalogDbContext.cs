using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.Models;
using Deveel.Math;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CelestialObjectCatalog.Persistence.Context
{
    public class CelestialObjectCatalogDbContext : DbContext
    {
        public DbSet<CelestialObject> CelestialObjects { get; set; }

        public DbSet<DiscoverySource> DiscoverySources { get; set; }

        public DbSet<CelestialObjectDiscovery> CelestialObjectDiscoveries { get; set; }


        public CelestialObjectCatalogDbContext(DbContextOptions<CelestialObjectCatalogDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //enable logging
            optionsBuilder
                .LogTo(
                    Console.WriteLine,
                    new[]
                    {
                        DbLoggerCategory.Database.Command.Name
                    },
                    LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create bif decimal value converter
            var bigDecimalValueConverter =
                new ValueConverter<BigDecimal, string>(
                    model => model.ToString(),
                    provider => BigDecimal.Parse(provider));
                    
            //discovery source
            modelBuilder
                .Entity<DiscoverySource>(entity =>
                {
                    //define the key
                    entity
                        .HasKey(ds => ds.DiscoverySourceId);

                    //define key value
                    entity
                        .Property(ds => ds.DiscoverySourceId)
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    //define index
                    entity
                        .HasIndex(ds => ds.StateOwner)
                        .IsClustered(false)
                        .HasDatabaseName("IX_DiscoverySources_StateOwner");

                    //add unique index on name
                    entity
                        .HasIndex(ds => ds.Name)
                        .IsUnique();
                });

            //celestial object
            modelBuilder
                .Entity<CelestialObject>(entity =>
                {
                    //define the key
                    entity
                        .HasKey(co => co.CelestialObjectId);

                    //define key value
                    entity
                        .Property(co => co.CelestialObjectId)
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    //define non clustered indexes
                    entity
                        .HasIndex(co => co.Name)
                        .IsClustered(false)
                        .HasDatabaseName("IX_CelestialObjects_Name");

                    entity
                        .HasIndex(co => co.Type)
                        .IsClustered(false)
                        .HasDatabaseName("IX_CelestialObjects_Type");

                    //define the unique index
                    entity
                        .HasIndex(x => x.Name)
                        .IsUnique();

                    //set the value converters (store in database as string and in model as big decimal)
                    entity
                        .Property(x => x.Mass)
                        .HasConversion(bigDecimalValueConverter);

                    entity
                        .Property(x => x.EquatorialDiameter)
                        .HasConversion(bigDecimalValueConverter);

                    entity
                        .Property(x => x.SurfaceTemperature)
                        .HasConversion(bigDecimalValueConverter);
                });

            //celestial object discovery
            modelBuilder
                .Entity<CelestialObjectDiscovery>(entity =>
                {
                    //define key
                    entity
                        .HasKey(cod => new
                        {
                            cod.CelestialObjectId,
                            cod.DiscoverySourceId
                        });

                    //define the one to many relation with CelestialObject entity
                    entity
                        .HasOne(co => co.CelestialObject)
                        .WithMany(c => c.CelestialObjectDiscoveries)
                        .HasForeignKey(co => co.CelestialObjectId);

                    //define the one to many relation with DiscoverySource
                    entity
                        .HasOne(c => c.DiscoverySource)
                        .WithMany(ds => ds.CelestialObjectDiscoveries)
                        .HasForeignKey(co => co.DiscoverySourceId);

                    //set default value on discovery date
                    entity
                        .Property(cod => cod.DiscoveryDate)
                        .HasDefaultValueSql("GETDATE()");
                });
        }
    }
}
