using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Task\";");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "TaskTag",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TaskTag",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TaskState",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "TaskState",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TaskState",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Task",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Task",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Task",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Task",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tag",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Tag",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Tag",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(
                        type: "character varying(50)",
                        maxLength: 50,
                        nullable: false
                    ),
                    HashedPassword = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: false
                    ),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    UpdatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    LastName = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    Email = table.Column<string>(
                        type: "character varying(255)",
                        maxLength: 255,
                        nullable: false
                    ),
                    Description = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: false,
                        defaultValue: ""
                    ),
                    PictureUrl = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    BannerUrl = table.Column<string>(
                        type: "character varying(500)",
                        maxLength: 500,
                        nullable: true
                    ),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    UpdatedAt = table.Column<DateTimeOffset>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("1ea6c135-be0f-4cd8-8029-863975601330"),
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("345b41fc-0019-46e0-b35d-e5a61ad76a4b"),
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("e280113f-a704-4e96-b909-30b322cc08b4"),
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: new Guid("ea56ef56-0c7b-4995-9f7f-1333a363a9db"),
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.UpdateData(
                table: "TaskState",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.UpdateData(
                table: "TaskState",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.UpdateData(
                table: "TaskState",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTimeOffset(
                    new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)
                )
            );

            migrationBuilder.CreateIndex(
                name: "IX_TaskState_Name",
                table: "TaskState",
                column: "Name",
                unique: true
            );

            migrationBuilder.CreateIndex(name: "IX_Task_UserId", table: "Task", column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name",
                table: "Tag",
                column: "Name",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Email",
                table: "UserProfile",
                column: "Email",
                unique: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Task_User_UserId",
                table: "Task",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Task_User_UserId", table: "Task");

            migrationBuilder.DropTable(name: "UserProfile");

            migrationBuilder.DropTable(name: "User");

            migrationBuilder.DropIndex(name: "IX_TaskState_Name", table: "TaskState");

            migrationBuilder.DropIndex(name: "IX_Task_UserId", table: "Task");

            migrationBuilder.DropIndex(name: "IX_Tag_Name", table: "Tag");

            migrationBuilder.DropColumn(name: "CreatedAt", table: "TaskTag");

            migrationBuilder.DropColumn(name: "UpdatedAt", table: "TaskTag");

            migrationBuilder.DropColumn(name: "CreatedAt", table: "TaskState");

            migrationBuilder.DropColumn(name: "UpdatedAt", table: "TaskState");

            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Task");

            migrationBuilder.DropColumn(name: "UserId", table: "Task");

            migrationBuilder.DropColumn(name: "CreatedAt", table: "Tag");

            migrationBuilder.DropColumn(name: "UpdatedAt", table: "Tag");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TaskState",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30
            );

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Task",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50
            );

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Task",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000
            );

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tag",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30
            );
        }
    }
}
