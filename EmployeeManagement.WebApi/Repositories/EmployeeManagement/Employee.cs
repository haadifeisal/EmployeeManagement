using System;
using System.Collections.Generic;

#nullable disable

namespace EmployeeManagement.WebApi.Repositories.EmployeeManagement
{
    public partial class Employee
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public long Salary { get; set; }
        public Guid DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}
