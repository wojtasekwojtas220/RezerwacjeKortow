using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RezerwacjeKortow.Data.Migrations
{
    /// <inheritdoc />
    public partial class InicjalizacjaKortow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: false),
                    CenaZaGodzine = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rezerwacje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataRozpoczecia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CzasTrwania = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    KortId = table.Column<int>(type: "INTEGER", nullable: false),
                    UzytkownikId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezerwacje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezerwacje_AspNetUsers_UzytkownikId",
                        column: x => x.UzytkownikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezerwacje_Korty_KortId",
                        column: x => x.KortId,
                        principalTable: "Korty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_KortId",
                table: "Rezerwacje",
                column: "KortId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_UzytkownikId",
                table: "Rezerwacje",
                column: "UzytkownikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezerwacje");

            migrationBuilder.DropTable(
                name: "Korty");
        }
    }
}
