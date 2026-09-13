using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rushea.Identity.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganisationNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                schema: "app",
                table: "organisations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                computedColumnSql: "lower(\"Name\")",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "ix_organisations_normalized_name",
                schema: "app",
                table: "organisations",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_organisations_normalized_name",
                schema: "app",
                table: "organisations");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                schema: "app",
                table: "organisations");
        }
    }
}
