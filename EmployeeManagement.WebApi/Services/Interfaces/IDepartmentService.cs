using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetDetpartments();
        Task<Department> GetDepartment(Guid departmentId);
        Task<Department> CreateDepartment(DepartmentRequestDto departmentRequestDto);
        Task<Department> UpdateDepartment(Guid departmentId, DepartmentRequestDto departmentRequestDto);
        Task<bool> DeleteDepartment(Guid departmentId);
    }
}
