using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR.Data.Migrations
{
    public partial class newTableForAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticationTicket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LastActivity = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Expires = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationTicket", x => x.Id);
                });

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationTicket");
                       
        }
    }
}
