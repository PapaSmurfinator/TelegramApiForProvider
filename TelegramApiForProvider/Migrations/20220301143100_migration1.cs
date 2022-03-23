using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramApiForProvider.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ChatId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
