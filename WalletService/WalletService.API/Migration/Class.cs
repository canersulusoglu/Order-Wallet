using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WalletService.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    DebtAmount = table.Column<double>(type: "double", nullable: false),
                    PaymentAmount = table.Column<double>(type: "double", nullable: false),
                    RoomName = table.Column<string>(type: "text", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderId", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserEmail",
                table: "Wallet",
                column: "UserEmail");
        }

        /* protected override void Down(MigrationBuilder migrationBuilder)
         {
             migrationBuilder.DropTable(
                 name: "Wallet");
     }*/
    }
}
