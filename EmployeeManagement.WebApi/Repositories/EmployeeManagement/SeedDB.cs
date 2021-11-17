using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Repositories.EmployeeManagement
{
    public static class SeedDB
    {
        public static void Populate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<EmployeeManagementContext>());
            }
        }

        public static void SeedData(EmployeeManagementContext context)
        {
            context.Database.Migrate();

            if (!context.Departments.Any())
            {
                System.Console.WriteLine("\n\nAdding data - Seeding ...\n\n");

                var department1 = new Department
                {
                    DepartmentId = Guid.NewGuid(),
                    Name = "IT"
                };
                context.Departments.Add(department1);

                var department2 = new Department
                {
                    DepartmentId = Guid.NewGuid(),
                    Name = "Finance"
                };
                context.Departments.Add(department2);

                var employee1 = new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    Name = "James",
                    Salary = 70000,
                    DepartmentId = department1.DepartmentId
                };
                context.Employees.Add(employee1);

                var employee2 = new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    Name = "Sara",
                    Salary = 82000,
                    DepartmentId = department2.DepartmentId
                };
                context.Employees.Add(employee2);

                context.SaveChanges();

                System.Console.WriteLine("\n\n Seeding Completed!! ...\n\n");
            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");
            }
        }

    }
}
