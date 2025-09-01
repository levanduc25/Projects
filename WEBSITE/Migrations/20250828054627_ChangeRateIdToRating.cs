using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBSITE.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRateIdToRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RateId",
                table: "Reviews",
                newName: "Rating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Reviews",
                newName: "RateId");
        }
    }
}
