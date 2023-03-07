using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _employeeManagementContext;

        public DepartmentService(IMapper mapper, EmployeeManagementContext employeeManagementContext)
        {
            _mapper = mapper;
            _employeeManagementContext = employeeManagementContext;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            var departments = await _employeeManagementContext.Departments.AsNoTracking().Include(x => x.Employees)
                .ToListAsync();

            return departments;
        }

        public async Task<Department> GetDepartment(Guid departmentId)
        {
            var department = await _employeeManagementContext.Departments.AsNoTracking().Include(x => x.Employees)
                .FirstOrDefaultAsync(x => x.DepartmentId == departmentId);

            return department;
        }

        public async Task<Department> CreateDepartment(DepartmentRequestDto departmentRequestDto)
        {
            var department = await _employeeManagementContext.Departments
                .FirstOrDefaultAsync(x => x.Name.ToLower() == departmentRequestDto.Name.ToLower());

            if (department != null)
            {
                return null;
            }

            var newDepartment = _mapper.Map<Department>(departmentRequestDto);
            newDepartment.DepartmentId = Guid.NewGuid();

            _employeeManagementContext.Departments.Add(newDepartment);
            _employeeManagementContext.SaveChanges();

            return newDepartment;
        }


        public async Task<Department> UpdateDepartment(Guid departmentId, DepartmentRequestDto departmentRequestDto)
        {
            var department = await CheckIfDepartmentExist(departmentId);

            if (department == null)
            {
                return null;
            }

            department.Name = string.IsNullOrEmpty(departmentRequestDto.Name) ? department.Name : departmentRequestDto.Name;

            await _employeeManagementContext.SaveChangesAsync();

            return department;
        }

        public async Task<bool> DeleteDepartment(Guid departmentId)
        {
            var department = await CheckIfDepartmentExist(departmentId);

            if(department == null)
            {
                return false;
            }

            var departmentHasEmployees = await _employeeManagementContext.Departments.AsNoTracking()
                .AnyAsync(x => x.DepartmentId == departmentId && x.Employees.Any());

            if (departmentHasEmployees)
            {
                return false;
            }

            _employeeManagementContext.Departments.Remove(department);
            await _employeeManagementContext.SaveChangesAsync();

            return true;
        }

        public async Task<Department> CheckIfDepartmentExist(Guid departmentId)
        {
            var department = await _employeeManagementContext.Departments
                .FirstOrDefaultAsync(x => x.DepartmentId == departmentId);

            return department;
        }
    }
}
