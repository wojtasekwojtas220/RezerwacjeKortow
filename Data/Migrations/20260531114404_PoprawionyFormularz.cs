using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RezerwacjeKortow.Data.Migrations
{
    /// <inheritdoc />
    public partial class PoprawionyFormularz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Miasto",
                table: "Rezerwacje",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Miasto",
                table: "Rezerwacje");
        }
    }
}
