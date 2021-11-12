using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.WebApi.DataTransferObjects.RequestDtos
{
    public class EmployeeRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public long Salary { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
    }
}
