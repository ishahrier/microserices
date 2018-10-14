using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductCatalogApi.Data.Migrations
{
    public partial class next : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "catalog_hilo");

            migrationBuilder.CreateSequence(
                name: "catalog_brand_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_item_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "catalog_brand_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_item_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_type_hilo");

            migrationBuilder.CreateSequence(
                name: "catalog_hilo",
                incrementBy: 10);
        }
    }
}
