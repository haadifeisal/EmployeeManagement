﻿using AutoMapper;
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
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly EmployeeManagementContext _employeeManagementContext;
        private readonly IDepartmentService _departmentService;

        public EmployeeService(IMapper mapper, EmployeeManagementContext employeeManagementContext,
            IDepartmentService departmentService)
        {
            _mapper = mapper;
            _employeeManagementContext = employeeManagementContext;
            _departmentService = departmentService;
        }
        
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var employees = await _employeeManagementContext.Employees.AsNoTracking().ToListAsync();

            return employees;
        }

        public async Task<Employee> GetEmployee(Guid employeeId)
        {
            var employee = await _employeeManagementContext.Employees
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            return employee;
        }

        public async Task<Employee> CreateEmployee(EmployeeRequestDto employeeRequestDto)
        {
            var department = await _departmentService.CheckIfDepartmentExist(employeeRequestDto.DepartmentId);

            if(department == null)
            {
                return null;
            }

            var newEmployee = _mapper.Map<Employee>(employeeRequestDto);
            newEmployee.EmployeeId = Guid.NewGuid();

            await _employeeManagementContext.Employees.AddAsync(newEmployee);
            await _employeeManagementContext.SaveChangesAsync();

            return newEmployee;
        }

        public async Task<Employee> UpdateEmployee(Guid employeeId, EmployeeRequestDto employeeRequestDto)
        {
            var employee = await GetEmployee(employeeId);

            if (employee == null)
            {
                return null;
            }

            var department = await _departmentService.CheckIfDepartmentExist(employeeRequestDto.DepartmentId);

            if (department == null)
            {
                return null;
            }

            employee.Name = string.IsNullOrEmpty(employeeRequestDto.Name) ? employee.Name : employeeRequestDto.Name;
            employee.Salary = employeeRequestDto.Salary;
            employee.DepartmentId = employeeRequestDto.DepartmentId;

            await _employeeManagementContext.SaveChangesAsync();

            return employee;
        }

        public async Task<bool> DeleteEmployee(Guid employeeId)
        {
            var employee = await GetEmployee(employeeId);

            if (employee == null)
            {
                return false;
            }

            _employeeManagementContext.Employees.Remove(employee);

            if(await _employeeManagementContext.SaveChangesAsync() == 1){
                return true;
            }

            return false;
        }
    }
}
