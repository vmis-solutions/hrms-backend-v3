using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HRMS.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IDBContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApprovalInfo> ApprovalInfos => Set<ApprovalInfo>();

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Company> Companies => Set<Company>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<JobTitle> JobTitles => Set<JobTitle>();

        public DbSet<LeaveApplication> LeaveApplications => Set<LeaveApplication>();


        public DbSet<Attendance> Attendances => Set<Attendance>();

        public DbSet<Payroll> Payrolls => Set<Payroll>();

        public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

        public DbSet<EmployeeDocs> EmployeeDocs => Set<EmployeeDocs>();

        DbSet<ApprovalInfo> IDBContext.ApprovalInfos => throw new NotImplementedException();

        DbSet<Employee> IDBContext.Employees => throw new NotImplementedException();

        DbSet<Company> IDBContext.Companies => throw new NotImplementedException();

        DbSet<Department> IDBContext.Departments => throw new NotImplementedException();

        DbSet<JobTitle> IDBContext.JobTitles => throw new NotImplementedException();

        DbSet<LeaveApplication> IDBContext.LeaveApplications => throw new NotImplementedException();

        DbSet<Attendance> IDBContext.Attendances => throw new NotImplementedException();

        DbSet<Payroll> IDBContext.Payrolls => throw new NotImplementedException();

        DbSet<LeaveBalance> IDBContext.LeaveBalances => throw new NotImplementedException();

        DbSet<EmployeeDocs> IDBContext.EmployeeDocs => throw new NotImplementedException();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.UseSnakeCaseNamingConvention();

            // Configure Entity Primary Keys and Base Properties
            ConfigureBaseEntities(modelBuilder);

            // Apply global query filter for soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseAuditable).IsAssignableFrom(e.ClrType) && !e.IsAbstract()))
            {
                var method = typeof(ApplicationDbContext).GetMethod(nameof(SetGlobalQuery), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(null, new object[] { modelBuilder });
            }

            // Configure Company
            ConfigureCompany(modelBuilder);

            // Configure Department
            ConfigureDepartment(modelBuilder);

            // Configure JobTitle
            ConfigureJobTitle(modelBuilder);

            // Configure Employee
            ConfigureEmployee(modelBuilder);

            // Configure LeaveApplication
            ConfigureLeaveApplication(modelBuilder);

            // Configure LeaveBalance
            ConfigureLeaveBalance(modelBuilder);

            // Configure Attendance
            ConfigureAttendance(modelBuilder);

            // Configure Payroll
            ConfigurePayroll(modelBuilder);

            // Configure EmployeeDocs
            ConfigureEmployeeDocs(modelBuilder);

            // Configure Indexes
            ConfigureIndexes(modelBuilder);

            // Seed Data (optional)
            SeedData(modelBuilder);
        }

        private static void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseAuditable
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ConfigureBaseEntities(ModelBuilder modelBuilder)
        {
            // Configure common properties for all entities inheriting from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => e.ClrType.IsSubclassOf(typeof(BaseAuditable))))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("Id")
                    .HasMaxLength(36);

                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedBy")
                    .HasMaxLength(36);

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedBy")
                    .HasMaxLength(36);
            }
        }

        private void ConfigureCompany(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(100);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20);

                // One-to-many: Company -> Departments
                entity.HasMany(c => c.Departments)
                    .WithOne(d => d.Company)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: Company -> Employees
                entity.HasMany(c => c.Employees)
                    .WithOne(e => e.Company)
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion
            });
        }

        private void ConfigureDepartment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.HeadEmployeeId)
                    .HasMaxLength(36);

                // One-to-many: Department -> Employees
                entity.HasMany(d => d.Employees)
                    .WithOne(e => e.Department)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-many: Department -> JobTitles
                entity.HasMany(d => d.JobTitles)
                    .WithOne(j => j.Department)
                    .HasForeignKey(j => j.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-one: Department -> Head Employee
                entity.HasOne(d => d.Head)
                    .WithMany(e => e.DepartmentsAsHead)
                    .HasForeignKey(d => d.HeadEmployeeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureJobTitle(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.DepartmentId)
                    .IsRequired()
                    .HasMaxLength(36);

                // One-to-many: JobTitle -> Employees
                entity.HasMany(j => j.Employees)
                    .WithOne(e => e.JobTitle)
                    .HasForeignKey(e => e.JobTitleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureEmployee(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Personal Info
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                // Government IDs
                entity.Property(e => e.SssNumber)
                    .HasMaxLength(15);

                entity.Property(e => e.PhilHealthNumber)
                    .HasMaxLength(15);

                entity.Property(e => e.PagIbigNumber)
                    .HasMaxLength(15);

                entity.Property(e => e.Tin)
                    .HasMaxLength(15);

                // Employment Info
                entity.Property(e => e.EmployeeNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CompanyId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.DepartmentId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.JobTitleId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(500);

                // Enum configurations
                entity.Property(e => e.Gender)
                    .HasConversion<string>()
                    .HasMaxLength(10);

                entity.Property(e => e.CivilStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(e => e.EmploymentStatus)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                // One-to-many: Employee -> LeaveApplications
                entity.HasMany(e => e.LeaveApplications)
                    .WithOne(l => l.Employee)
                    .HasForeignKey(l => l.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: Employee -> Attendances
                entity.HasMany(e => e.Attendances)
                    .WithOne(a => a.Employee)
                    .HasForeignKey(a => a.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // One-to-many: Employee -> Payrolls
                entity.HasMany(e => e.Payrolls)
                    .WithOne(p => p.Employee)
                    .HasForeignKey(p => p.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureLeaveApplication(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveApplication>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.DepartmentHeadId)
                    .HasMaxLength(36);

                entity.Property(e => e.HrPersonnelId)
                    .HasMaxLength(36);

                // Enum configurations
                entity.Property(e => e.LeaveType)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                // Configure owned entities for approval info
                entity.OwnsOne(e => e.DepartmentHeadApproval, approval =>
                {
                    approval.Property(a => a.ApprovedBy)
                        .HasMaxLength(36);

                    approval.Property(a => a.Comments)
                        .HasMaxLength(1000);
                });

                entity.OwnsOne(e => e.HrAcknowledgment, acknowledgment =>
                {
                    acknowledgment.Property(a => a.ApprovedBy)
                        .HasMaxLength(36);

                    acknowledgment.Property(a => a.Comments)
                        .HasMaxLength(1000);
                });

                entity.Property(e => e.LeaveDays)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<LeaveDay>>(v, (JsonSerializerOptions)null) ?? new List<LeaveDay>())
                    .Metadata.SetValueComparer(new ValueComparer<List<LeaveDay>>(
                        (c1, c2) => JsonSerializer.Serialize(c1, (JsonSerializerOptions)null) == JsonSerializer.Serialize(c2, (JsonSerializerOptions)null),
                        c => c == null ? 0 : JsonSerializer.Serialize(c, (JsonSerializerOptions)null).GetHashCode(),
                        c => JsonSerializer.Deserialize<List<LeaveDay>>(JsonSerializer.Serialize(c, (JsonSerializerOptions)null), (JsonSerializerOptions)null)!
                    ));

                // Configure relationships with department head and HR personnel
                entity.HasOne(l => l.DepartmentHead)
                    .WithMany(e => e.LeaveApplicationsAsDepartmentHead)
                    .HasForeignKey(l => l.DepartmentHeadId)
                    .OnDelete(DeleteBehavior.Restrict);
                //.OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(l => l.HrPersonnel)
                    .WithMany(e => e.LeaveApplicationsAsHrPersonnel)
                    .HasForeignKey(l => l.HrPersonnelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureLeaveBalance(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveBalance>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(36);

                // One-to-many: Employee -> LeaveBalances
                entity.HasOne(l => l.Employee)
                    .WithMany() // Assuming no navigation property in Employee for LeaveBalances
                    .HasForeignKey(l => l.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureAttendance(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.TotalHours)
                    .HasPrecision(5, 2); // 999.99 hours max
            });
        }

        private void ConfigurePayroll(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(36);

                // Configure decimal precision for money values
                entity.Property(e => e.BasicPay)
                    .HasPrecision(18, 2);

                entity.Property(e => e.Allowances)
                    .HasPrecision(18, 2);

                entity.Property(e => e.Deductions)
                    .HasPrecision(18, 2);

                entity.Property(e => e.NetPay)
                    .HasPrecision(18, 2);
            });
        }

        private void ConfigureEmployeeDocs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDocs>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DocumentName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DocumentType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DocumentDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.DocumentPath)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasMaxLength(36);

                // One-to-many: Employee -> EmployeeDocs
                entity.HasOne(d => d.Employee)
                    .WithMany() // Assuming no navigation property in Employee for EmployeeDocs
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // Company indexes
            modelBuilder.Entity<Company>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Employee indexes
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeNumber)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.CompanyId, e.DepartmentId });

            // Government ID indexes (unique but allow nulls)
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.SssNumber)
                .IsUnique()
                .HasFilter("\"SssNumber\" IS NOT NULL");

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.PhilHealthNumber)
                .IsUnique()
                .HasFilter("\"PhilHealthNumber\" IS NOT NULL");

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.PagIbigNumber)
                .IsUnique()
                .HasFilter("\"PagIbigNumber\" IS NOT NULL");

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Tin)
                .IsUnique()
                .HasFilter("\"Tin\" IS NOT NULL");

            // Leave application indexes
            modelBuilder.Entity<LeaveApplication>()
                .HasIndex(l => new { l.EmployeeId, l.Status });

            modelBuilder.Entity<LeaveApplication>()
                .HasIndex(l => new { l.StartDate, l.EndDate });

            // Leave balance indexes
            modelBuilder.Entity<LeaveBalance>()
                .HasIndex(l => new { l.EmployeeId, l.Year })
                .IsUnique();

            // Attendance indexes
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.EmployeeId, a.Date })
                .IsUnique();

            // Payroll indexes
            modelBuilder.Entity<Payroll>()
                .HasIndex(p => new { p.EmployeeId, p.PeriodStart, p.PeriodEnd });
        }


        private void SeedData(ModelBuilder modelBuilder)
        {
            // Predefined GUIDs
            Guid companyId = Guid.Parse("3d6a9e77-b6c6-4c1a-a4b2-7b92f413f9a9");
            Guid departmentId = Guid.Parse("b2d3ae87-3e6d-4122-b007-304cd84089a1");
            Guid jobtitleId = Guid.Parse("7c4229e8-bad7-406c-80b0-eaa4d56184f1");
            Guid headEmployeeId = Guid.Parse("e5d5fca1-b75b-46ec-8fcb-ef2f8b499e00");

            var CreatedAt = new DateTime(2024, 06, 24, 0, 0, 0, DateTimeKind.Utc);
            var UpdatedAt = new DateTime(2024, 06, 24, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() }
                , new IdentityRole { Id = "25ab6d7e-585f-469e-902b-f60008bdfb03", Name = "Developer", NormalizedName = "DEVELOPER".ToUpper() }
                , new IdentityRole { Id = "9911b550-e25d-4889-8d72-df82d884e7b7", Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper() }
                , new IdentityRole { Id = "8d69da53-ca1b-4c83-85d8-99bd7fa9836c", Name = "Employee", NormalizedName = "EMPLOYEE".ToUpper() });

            //Seeding the User to AspNetUsers table
            

            // 🏢 Company
            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = companyId,
                Name = "Default Company",
                Description = "Default company for initial setup",
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            });

            // 🏬 Department (seed with HeadEmployeeId = null to break cycle)
            modelBuilder.Entity<Department>().HasData(new Department
            {
                Id = departmentId,
                Name = "Human Resources",
                Description = "Human Resources Department",
                CompanyId = companyId,
                HeadEmployeeId = null, // Set to null for initial seed
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            });

            // 👔 Job Title (after Department exists)
            modelBuilder.Entity<JobTitle>().HasData(new JobTitle
            {
                Id = jobtitleId,
                Title = "HR Manager",
                Description = "Human Resources Manager",
                DepartmentId = departmentId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            });

            // 👨‍💼 Employee (after Department and JobTitle exist)
            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                Id = headEmployeeId,
                UserId = "user-hr-head",
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Gender = Gender.Male,
                CivilStatus = CivilStatus.Single,
                PhoneNumber = "09171234567",
                Address = "HR Building",
                SssNumber = "123456789",
                PhilHealthNumber = "987654321",
                PagIbigNumber = "456789123",
                Tin = "321654987",
                EmployeeNumber = "EMP-0001",
                DateHired = CreatedAt,
                CompanyId = companyId,
                DepartmentId = departmentId,
                JobTitleId = jobtitleId,
                EmploymentStatus = EmploymentStatus.Regular,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            });

            // NOTE: After initial migration, update Department.HeadEmployeeId to headEmployeeId via a follow-up migration or manual SQL if needed.
        }

    }
}
