using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMetadatastrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConfigValue",
                value: "{station}_{genre}_{title}_{publishdate}_{publishtime}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConfigValue",
                value: "{station}_{genre}_{title}_{date}_{time}");
        }
    }
}
