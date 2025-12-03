using HRMS.Application.Interfaces.Departments;
using HRMS.Application.Interfaces.Companies;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(Guid id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
        }

        public async Task DeleteAsync(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
        }

        public async Task<Department?> GetByIdWithHrManagersAsync(Guid id)
        {
            return await _context.Departments
                .Include(d => d.HrManagers)
                    .ThenInclude(dhm => dhm.Employee)
                .Include(d => d.Employees)
                .Include(d => d.Company)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<DepartmentHrManager>> GetHrManagersByDepartmentIdAsync(Guid departmentId)
        {
            return await _context.DepartmentHrManagers
                .Include(dhm => dhm.Employee)
                .Where(dhm => dhm.DepartmentId == departmentId && !dhm.IsDeleted)
                .ToListAsync();
        }

        public async Task<DepartmentHrManager?> GetHrManagerByIdAsync(Guid id)
        {
            return await _context.DepartmentHrManagers
                .Include(dhm => dhm.Employee)
                .Include(dhm => dhm.Department)
                .FirstOrDefaultAsync(dhm => dhm.Id == id && !dhm.IsDeleted);
        }

        public async Task<DepartmentHrManager?> GetHrManagerByDepartmentAndEmployeeAsync(Guid departmentId, Guid employeeId)
        {
            // First check the change tracker for entities in any state (Added, Modified, Unchanged, Deleted)
            var trackedEntry = _context.ChangeTracker.Entries<DepartmentHrManager>()
                .FirstOrDefault(e => e.Entity.DepartmentId == departmentId 
                    && e.Entity.EmployeeId == employeeId);

            if (trackedEntry != null)
            {
                // If entity is marked as deleted, we still return it so it can be restored
                return trackedEntry.Entity;
            }

            // Then check the database (this will include soft-deleted records)
            // Note: We don't use AsNoTracking() here because we may need to update the entity
            return await _context.DepartmentHrManagers
                .FirstOrDefaultAsync(dhm => dhm.DepartmentId == departmentId 
                    && dhm.EmployeeId == employeeId);
        }

        public async Task AddHrManagerAsync(DepartmentHrManager departmentHrManager)
        {
            // Check change tracker first for immediate duplicates
            var trackedEntry = _context.ChangeTracker.Entries<DepartmentHrManager>()
                .FirstOrDefault(e => e.Entity.DepartmentId == departmentHrManager.DepartmentId 
                    && e.Entity.EmployeeId == departmentHrManager.EmployeeId);

            if (trackedEntry != null)
            {
                var existing = trackedEntry.Entity;
                // If entity is being added, we have a duplicate in the same transaction
                if (trackedEntry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                {
                    // Don't add duplicate
                    return;
                }
                // If exists and is deleted, restore it instead
                if (existing.IsDeleted)
                {
                    existing.IsDeleted = false;
                    existing.UpdatedAt = DateTime.UtcNow;
                    _context.DepartmentHrManagers.Update(existing);
                    return;
                }
                // If exists and not deleted, don't add (already assigned)
                return;
            }

            // Check database for existing records (track it so we can update if needed)
            var dbRecord = await _context.DepartmentHrManagers
                .FirstOrDefaultAsync(dhm => dhm.DepartmentId == departmentHrManager.DepartmentId 
                    && dhm.EmployeeId == departmentHrManager.EmployeeId);
            
            if (dbRecord != null)
            {
                // If exists in DB and is deleted, restore it
                if (dbRecord.IsDeleted)
                {
                    dbRecord.IsDeleted = false;
                    dbRecord.UpdatedAt = DateTime.UtcNow;
                    _context.DepartmentHrManagers.Update(dbRecord);
                    return;
                }
                // If exists and not deleted, don't add (already assigned)
                return;
            }

            // Safe to add new record
            await _context.DepartmentHrManagers.AddAsync(departmentHrManager);
        }

        public async Task UpdateHrManagerAsync(DepartmentHrManager departmentHrManager)
        {
            _context.DepartmentHrManagers.Update(departmentHrManager);
        }

        public async Task RemoveHrManagerAsync(Guid id)
        {
            var hrManager = await _context.DepartmentHrManagers.FindAsync(id);
            if (hrManager != null)
            {
                // Permanent delete - remove from database
                _context.DepartmentHrManagers.Remove(hrManager);
            }
        }

        public async Task<bool> HrManagerExistsAsync(Guid departmentId, Guid employeeId)
        {
            return await _context.DepartmentHrManagers
                .AnyAsync(dhm => dhm.DepartmentId == departmentId 
                    && dhm.EmployeeId == employeeId 
                    && !dhm.IsDeleted);
        }

        public async Task<List<Department>> GetDepartmentsManagedByUserAsync(string userId)
        {
            return await _context.Departments
                .Include(d => d.HrManagers)
                    .ThenInclude(dhm => dhm.Employee)
                .Include(d => d.Employees)
                .Include(d => d.Company)
                .Where(d => !d.IsDeleted && d.HrManagers.Any(dhm =>
                    !dhm.IsDeleted && dhm.Employee.UserId == userId))
                .ToListAsync();
        }
    }
} 