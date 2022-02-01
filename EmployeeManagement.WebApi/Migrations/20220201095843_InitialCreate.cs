using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.WebApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    departmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    salary = table.Column<long>(type: "bigint", nullable: false),
                    departmentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.employeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Department",
                        column: x => x.departmentId,
                        principalTable: "Department",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_departmentId",
                table: "Employee",
                column: "departmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
