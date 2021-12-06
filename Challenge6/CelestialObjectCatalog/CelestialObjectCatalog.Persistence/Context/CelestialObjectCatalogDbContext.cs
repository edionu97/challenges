using Microsoft.EntityFrameworkCore;
using CelestialObjectCatalog.Persistence.Models;

namespace CelestialObjectCatalog.Persistence.Context
{
    public class CelestialObjectCatalogDbContext : DbContext
    {
        public DbSet<CelestialObject> CelestialObjects { get; set; }

        public DbSet<DiscoverySource> DiscoverySources { get; set; }

        public DbSet<CelestialObjectDiscovery> CelestialObjectDiscoveries{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=DESKTOP-VQ4KD11;Initial Catalog=CelestialObjectCatalog;Integrated Security=True;");
        }
    }
}
