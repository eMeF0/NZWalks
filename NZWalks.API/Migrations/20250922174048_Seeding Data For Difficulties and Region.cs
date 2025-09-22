using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataForDifficultiesandRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("243ac72b-8bc8-4d6b-a2a7-50217cf943db"), "Hard" },
                    { new Guid("2d17839f-9667-43d9-a04c-44f53341afbf"), "Medium" },
                    { new Guid("4d1e4610-637c-4137-ad19-211851045a11"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("1e6f5f3e-3b2a-4c4c-8a8a-1a1a1a1a1a1a"), "AKL", "Auckland", "img/auckland.jpg" },
                    { new Guid("2b7e5f3e-4c3b-5d5c-9b9b-2b2b2b2b2b2b"), "WLG", "Wellington", null },
                    { new Guid("3c8f6f4e-5d4c-6e6d-0c0c-3c3c3c3c3c3c"), "CHC", "Christchurch", "img/christchurch.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("243ac72b-8bc8-4d6b-a2a7-50217cf943db"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("2d17839f-9667-43d9-a04c-44f53341afbf"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("4d1e4610-637c-4137-ad19-211851045a11"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("1e6f5f3e-3b2a-4c4c-8a8a-1a1a1a1a1a1a"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("2b7e5f3e-4c3b-5d5c-9b9b-2b2b2b2b2b2b"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("3c8f6f4e-5d4c-6e6d-0c0c-3c3c3c3c3c3c"));
        }
    }
}
