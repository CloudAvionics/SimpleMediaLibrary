using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAppConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecordingsDirectory",
                table: "AppConfigs",
                newName: "ConfigValue");

            migrationBuilder.AddColumn<string>(
                name: "ConfigName",
                table: "AppConfigs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Id", "ConfigName", "ConfigValue" },
                values: new object[] { 1, "RecordingDir", "Recordings" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "ConfigName",
                table: "AppConfigs");

            migrationBuilder.RenameColumn(
                name: "ConfigValue",
                table: "AppConfigs",
                newName: "RecordingsDirectory");
        }
    }
}
