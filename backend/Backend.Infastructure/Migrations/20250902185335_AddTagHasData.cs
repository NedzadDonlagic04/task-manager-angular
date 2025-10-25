using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTagHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1ea6c135-be0f-4cd8-8029-863975601330"), "hobby" },
                    { new Guid("345b41fc-0019-46e0-b35d-e5a61ad76a4b"), "school" },
                    { new Guid("e280113f-a704-4e96-b909-30b322cc08b4"), "job" },
                    { new Guid("ea56ef56-0c7b-4995-9f7f-1333a363a9db"), "house" },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("1ea6c135-be0f-4cd8-8029-863975601330")
            );

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("345b41fc-0019-46e0-b35d-e5a61ad76a4b")
            );

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("e280113f-a704-4e96-b909-30b322cc08b4")
            );

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("ea56ef56-0c7b-4995-9f7f-1333a363a9db")
            );
        }
    }
}
