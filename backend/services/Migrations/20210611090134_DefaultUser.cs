using Microsoft.EntityFrameworkCore.Migrations;

namespace services.Migrations
{
    public partial class DefaultUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "ID", "Email", "Firstname", "Lastname", "Nickname", "PasswordHash", "Role" },
                values: new object[] { 1, "alexander.stuckenholz@hshl.de", "Alexander", "Stuckenholz", "Stucki", null, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
