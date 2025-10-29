using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTaskStateIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Task_TaskState_TaskStateId", table: "Task");

            migrationBuilder.DropColumn(name: "TaskStateId", table: "Task");

            migrationBuilder.AddColumn<int>(
                name: "TaskStateId",
                table: "Task",
                type: "integer",
                nullable: false
            );

            migrationBuilder.DropTable(name: "TaskState");

            migrationBuilder.CreateTable(
                name: "TaskState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskState", x => x.Id);
                }
            );

            migrationBuilder.InsertData(
                table: "TaskState",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Success" },
                    { 3, "Fail" },
                }
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Task_TaskState_TaskStateId",
                table: "Task",
                column: "TaskStateId",
                principalTable: "TaskState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "TaskState", keyColumn: "Id", keyValue: 1);

            migrationBuilder.DeleteData(table: "TaskState", keyColumn: "Id", keyValue: 2);

            migrationBuilder.DeleteData(table: "TaskState", keyColumn: "Id", keyValue: 3);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TaskState",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskStateId",
                table: "Task",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );
        }
    }
}
