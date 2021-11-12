using System;
using System.Collections.Generic;

#nullable disable

namespace EmployeeManagement.WebApi.Repositories.EmployeeManagement
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public Guid DepartmentId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
