using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace timeTrackingSystemBackend.Migrations
{
    public partial class WebApiDatabaseContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tunnit",
                columns: table => new
                {
                    TunnitID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LuokkahuoneID = table.Column<string>(maxLength: 4, nullable: false),
                    Sisaan = table.Column<DateTime>(type: "datetime", nullable: true),
                    Ulos = table.Column<DateTime>(type: "datetime", nullable: true),
                    OppilasID = table.Column<int>(nullable: true),
                    UserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tunnit", x => x.TunnitID);
                    table.ForeignKey(
                        name: "FK_tunnit_Users",
                        column: x => x.OppilasID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tunnit_OppilasID",
                table: "tunnit",
                column: "OppilasID");

            migrationBuilder.CreateIndex(
                name: "IX_tunnit",
                table: "tunnit",
                column: "TunnitID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tunnit");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
