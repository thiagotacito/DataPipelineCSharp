using DataPipeline.Domain.Entities.DataTraffic;
using DataPipeline.Domain.Entities.DataTraffic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataPipeline.Infrastructure.Data
{
    public  class DataPipelineDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        public DataPipelineDbContext(DbContextOptions<DataPipelineDbContext> options, 
                                    ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        //TODO: Study OnModelCreating for multi-tenancy
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Place).IsRequired().HasMaxLength(100);
                entity.Property(e => e.City).IsRequired().HasMaxLength(50);
                entity.Property(e => e.State).IsRequired().HasMaxLength(2);
                entity.Property(e => e.PaidAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.VehicleType).IsRequired().HasConversion<string>();
                entity.Property(e => e.IsOfficialVehicle).IsRequired().HasDefaultValue(false);
                entity.Property<Guid>("TenantId"); 
                entity.HasIndex("TenantId");
            });
            // Apply a global query filter to enforce multi-tenancy
            modelBuilder.Entity<Vehicle>().HasQueryFilter(v => EF.Property<Guid>(v, "TenantId") == _tenantProvider.TenantId);
        }
    }
}
