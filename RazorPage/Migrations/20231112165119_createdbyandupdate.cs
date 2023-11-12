using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RazorPage.Migrations
{
    public partial class createdbyandupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "Products");
        }
    }
}
