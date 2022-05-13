using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rentalAppAPI.DAL.Migrations
{
    public partial class AddedIdentificationString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentificationString",
                table: "Services",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificationString",
                table: "Services");
        }
    }
}
