using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.DataTransferObjects.RequestDtos
{
    public class EmployeeRequestDto
    {
        public string Name { get; set; }
        public long Salary { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
