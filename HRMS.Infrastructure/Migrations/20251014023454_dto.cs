using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaveType",
                table: "LeaveBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "LeaveBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedDays",
                table: "LeaveBalance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "LeaveApplication",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "LeaveApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "LeaveApplication",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "EmployeeDocs",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "EmployeeDocs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "EmployeeDocs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedDate",
                table: "EmployeeDocs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveType",
                table: "LeaveBalance");

            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "LeaveBalance");

            migrationBuilder.DropColumn(
                name: "UsedDays",
                table: "LeaveBalance");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "LeaveApplication");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "LeaveApplication");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "LeaveApplication");

            migrationBuilder.DropColumn(
                name: "File",
                table: "EmployeeDocs");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "EmployeeDocs");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "EmployeeDocs");

            migrationBuilder.DropColumn(
                name: "UploadedDate",
                table: "EmployeeDocs");
        }
    }
}
