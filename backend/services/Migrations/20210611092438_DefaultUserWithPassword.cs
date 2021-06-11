using Microsoft.EntityFrameworkCore.Migrations;

namespace services.Migrations
{
    public partial class DefaultUserWithPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "PasswordHash",
                value: "Rfc2898DeriveBytes$50000$oIDLniYnvmLaC5b7c8K8sg==$T6uvqqhpCEKrdwMmiHH2tbEz/iGc+mHfYrMxPNfNTug=");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "PasswordHash",
                value: null);
        }
    }
}
