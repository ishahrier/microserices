using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogApi.Domain;

namespace ProductCatalogApi.Data {
    public class CatalogContext : DbContext {
        public CatalogContext (DbContextOptions options) : base (options) {

        }

        protected override void OnModelCreating (ModelBuilder builder) {
            builder.Entity<CatalogBrand> (ConfigureCatalogBrand);
            builder.Entity<CatalogItem> (ConfigureCatalogItem);
            builder.Entity<CatalogType> (ConfigureCatalogType);
        }

        private void ConfigureCatalogType (EntityTypeBuilder<CatalogType> builder) {
            builder.ToTable ("CatlogType");
            builder.Property (x => x.Id)
                .ForSqlServerUseSequenceHiLo ("catalog_hilo")
                .IsRequired (true);
            builder.Property (x => x.Type)
                .IsRequired (true)
                .HasMaxLength (100);
        }

        private void ConfigureCatalogItem (EntityTypeBuilder<CatalogItem> builder) {
            builder.ToTable ("Catalog");
            builder.Property (x => x.Id)
                .ForSqlServerUseSequenceHiLo ("catalog_hilo")
                .IsRequired (true);
            builder.Property (x => x.Name)
                .IsRequired (true)
                .HasMaxLength (50);
            builder.Property (x => x.Price)
                .IsRequired (true);
            builder.Property (x => x.PictureUrl)
                .IsRequired (false);
            builder.HasOne (x => x.CatalogBrand)
                .WithMany ()
                .HasForeignKey (x => x.CatalogBrandId);
            builder.HasOne (x => x.CatalogType)
                .WithMany ()
                .HasForeignKey (x => x.CatalogTypeId);
        }

        private void ConfigureCatalogBrand (EntityTypeBuilder<CatalogBrand> builder) {
            builder.ToTable ("CatlogBrand");
            builder.Property (x => x.Id)
                .ForSqlServerUseSequenceHiLo ("catalog_hilo")
                .IsRequired (true);
            builder.Property (x => x.Brand)
                .IsRequired (true)
                .HasMaxLength (100);
        }

        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }

    }
}