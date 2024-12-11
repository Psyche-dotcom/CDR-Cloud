using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR.Data.Migrations
{
    public partial class newAttributeAddedToPromotionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicenseKey",
                table: "PromotionUsages",
                type: "nvarchar(max)",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseKey",
                table: "PromotionUsages");

           
        }
    }
}
