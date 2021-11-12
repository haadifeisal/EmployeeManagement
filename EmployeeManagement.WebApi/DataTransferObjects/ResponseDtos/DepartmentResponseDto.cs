using System;
using System.Collections.Generic;

namespace EmployeeManagement.WebApi.DataTransferObjects.ResponseDtos
{
    public class DepartmentResponseDto
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; }
        public IEnumerable<EmployeeResponseDto> Employees { get; set; }
    }
}
