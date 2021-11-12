using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.WebApi.DataTransferObjects.RequestDtos
{
    public class DepartmentRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
