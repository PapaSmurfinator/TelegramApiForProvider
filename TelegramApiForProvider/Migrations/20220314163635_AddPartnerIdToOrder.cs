using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TelegramApiForProvider.Migrations
{
    public partial class AddPartnerIdToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "ExtendedOrders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "ExtendedOrders");
        }
    }
}
