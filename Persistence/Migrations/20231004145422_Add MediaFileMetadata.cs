using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaFileMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MetadataId",
                table: "MediaFiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaFilesMetadatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Station = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PublishTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Genre = table.Column<string>(type: "TEXT", nullable: true),
                    Frequency = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFilesMetadatas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Id", "ConfigName", "ConfigValue" },
                values: new object[,]
                {
                    { 2, "ApplicationName", "Simple Media Library" },
                    { 3, "CompanyName", "CloudAvionics" },
                    { 4, "MediaFileNamingConvention", "CloudAvionics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MetadataId",
                table: "MediaFiles",
                column: "MetadataId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaFiles_MediaFilesMetadatas_MetadataId",
                table: "MediaFiles",
                column: "MetadataId",
                principalTable: "MediaFilesMetadatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaFiles_MediaFilesMetadatas_MetadataId",
                table: "MediaFiles");

            migrationBuilder.DropTable(
                name: "MediaFilesMetadatas");

            migrationBuilder.DropIndex(
                name: "IX_MediaFiles_MetadataId",
                table: "MediaFiles");

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "MetadataId",
                table: "MediaFiles");
        }
    }
}
