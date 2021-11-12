using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Services.Interfaces
{
    public interface IEmployeeService 
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployee(Guid employeeId);
        Task<Employee> CreateEmployee(EmployeeRequestDto employeeRequestDto);
        Task<Employee> UpdateEmployee(Guid employeeId, EmployeeRequestDto employeeRequestDto);
        Task<bool> DeleteEmployee(Guid employeeId);
    }
}
