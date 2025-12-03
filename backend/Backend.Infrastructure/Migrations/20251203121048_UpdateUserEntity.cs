using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserProfile",
                keyColumn: "UserId",
                keyValue: new Guid("9d07ca30-d8f9-40b7-b922-82f567ec6704")
            );

            migrationBuilder.DropColumn(name: "UserProfileId", table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[]
                {
                    "UserId",
                    "BannerUrl",
                    "CreatedAt",
                    "Description",
                    "Email",
                    "FirstName",
                    "LastName",
                    "PictureUrl",
                },
                values: new object[]
                {
                    new Guid("9d07ca30-d8f9-40b7-b922-82f567ec6704"),
                    null,
                    new DateTimeOffset(
                        new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                        new TimeSpan(0, 0, 0, 0, 0)
                    ),
                    "",
                    "mock@example.com",
                    "Mock",
                    "Mock",
                    null,
                }
            );
        }
    }
}
