using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ContactEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TimeOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalHours = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    HeadEmployeeId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitles_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CivilStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SssNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PhilHealthNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PagIbigNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Tin = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    EmployeeNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DateHired = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    EmploymentStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Avatar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_JobTitles_JobTitleId",
                        column: x => x.JobTitleId,
                        principalTable: "JobTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    LeaveType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalDays = table.Column<int>(type: "integer", nullable: false),
                    PaidDays = table.Column<int>(type: "integer", nullable: false),
                    UnpaidDays = table.Column<int>(type: "integer", nullable: false),
                    LeaveDays = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    AppliedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DepartmentHeadId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    HrPersonnelId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    DepartmentHeadApproval_ApprovedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    DepartmentHeadApproval_ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartmentHeadApproval_Comments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HrAcknowledgment_ApprovedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    HrAcknowledgment_ApprovedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HrAcknowledgment_Comments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveApplications_Employees_DepartmentHeadId",
                        column: x => x.DepartmentHeadId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LeaveApplications_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveApplications_Employees_HrPersonnelId",
                        column: x => x.HrPersonnelId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LeaveBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    TotalPaidLeave = table.Column<int>(type: "integer", nullable: false),
                    UsedPaidLeave = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveBalances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BasicPay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Allowances = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Deductions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NetPay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payrolls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "ContactEmail", "ContactPhone", "CreatedAt", "CreatedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("3d6a9e77-b6c6-4c1a-a4b2-7b92f413f9a9"), null, null, null, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "Default company for initial setup", "Default Company", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "CreatedBy", "Description", "HeadEmployeeId", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("b2d3ae87-3e6d-4122-b007-304cd84089a1"), new Guid("3d6a9e77-b6c6-4c1a-a4b2-7b92f413f9a9"), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "Human Resources Department", null, "Human Resources", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "JobTitles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DepartmentId", "Description", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("7c4229e8-bad7-406c-80b0-eaa4d56184f1"), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, new Guid("b2d3ae87-3e6d-4122-b007-304cd84089a1"), "Human Resources Manager", "HR Manager", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "Avatar", "BirthDate", "CivilStatus", "CompanyId", "CreatedAt", "CreatedBy", "DateHired", "DepartmentId", "Email", "EmployeeNumber", "EmploymentStatus", "FirstName", "Gender", "JobTitleId", "LastName", "MiddleName", "PagIbigNumber", "PhilHealthNumber", "PhoneNumber", "SssNumber", "Tin", "UpdatedAt", "UpdatedBy", "UserId" },
                values: new object[] { new Guid("e5d5fca1-b75b-46ec-8fcb-ef2f8b499e00"), "HR Building", null, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Single", new Guid("3d6a9e77-b6c6-4c1a-a4b2-7b92f413f9a9"), new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("b2d3ae87-3e6d-4122-b007-304cd84089a1"), "johndoe@example.com", "EMP-0001", "Regular", "John", "Male", new Guid("7c4229e8-bad7-406c-80b0-eaa4d56184f1"), "Doe", null, "456789123", "987654321", "09171234567", "123456789", "321654987", new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "user-hr-head" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId_Date",
                table: "Attendances",
                columns: new[] { "EmployeeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HeadEmployeeId",
                table: "Departments",
                column: "HeadEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId_DepartmentId",
                table: "Employees",
                columns: new[] { "CompanyId", "DepartmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitleId",
                table: "Employees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PagIbigNumber",
                table: "Employees",
                column: "PagIbigNumber",
                unique: true,
                filter: "\"PagIbigNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhilHealthNumber",
                table: "Employees",
                column: "PhilHealthNumber",
                unique: true,
                filter: "\"PhilHealthNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SssNumber",
                table: "Employees",
                column: "SssNumber",
                unique: true,
                filter: "\"SssNumber\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Tin",
                table: "Employees",
                column: "Tin",
                unique: true,
                filter: "\"Tin\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_DepartmentId",
                table: "JobTitles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_DepartmentHeadId",
                table: "LeaveApplications",
                column: "DepartmentHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_EmployeeId_Status",
                table: "LeaveApplications",
                columns: new[] { "EmployeeId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_HrPersonnelId",
                table: "LeaveApplications",
                column: "HrPersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_StartDate_EndDate",
                table: "LeaveApplications",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_EmployeeId_Year",
                table: "LeaveBalances",
                columns: new[] { "EmployeeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmployeeId_PeriodStart_PeriodEnd",
                table: "Payrolls",
                columns: new[] { "EmployeeId", "PeriodStart", "PeriodEnd" });

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employees_EmployeeId",
                table: "Attendances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_HeadEmployeeId",
                table: "Departments",
                column: "HeadEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_HeadEmployeeId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "LeaveApplications");

            migrationBuilder.DropTable(
                name: "LeaveBalances");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "JobTitles");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
