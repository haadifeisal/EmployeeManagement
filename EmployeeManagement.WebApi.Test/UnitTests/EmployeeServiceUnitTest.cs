﻿using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FakeItEasy;
using EmployeeManagement.WebApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Test.UnitTests
{
    [TestClass]
    public class EmployeeServiceUnitTest : UnitTestBase
    {
        [TestMethod]
        public async Task GetEmployees_WhenTheresEmployeeData_ShouldReturnCollectionOf2EmployeeObjects()
        {
            //Arrange
            var department = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(department);

            var employee1 = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = department.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(employee1);

            var employee2 = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Adam",
                Salary = 47000,
                DepartmentId = department.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(employee2);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.GetDetpartments()).Returns(new List<Department>());

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employees = await employeeService.GetEmployees();

            //Assert
            Assert.AreEqual(2, employees.Count());
        }

        [TestMethod]
        public async Task GetEmployees_WhenNoEmployeeDataIsFound_ShouldReturnEmptyCollection()
        {
            //Arrange

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.GetDetpartments()).Returns(new List<Department>());

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employees = await employeeService.GetEmployees();

            //Assert
            Assert.IsTrue(!employees.Any());
        }

        [TestMethod]
        public async Task GetEmployee_WhenEmployeeIsFound_ShouldReturnEmployeeObject()
        {
            //Arrange
            var department = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(department);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = department.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.GetDetpartments()).Returns(new List<Department>());

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.GetEmployee(newEmployee.EmployeeId);

            //Assert
            Assert.IsNotNull(employee);
            Assert.AreEqual(newEmployee.EmployeeId, employee.EmployeeId);
            Assert.AreEqual(newEmployee.Name, employee.Name);
        }

        [TestMethod]
        public async Task GetEmployee_WhenEmployeeIsNotFound_ShouldReturnNull()
        {
            //Arrange
            var department = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(department);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = department.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.GetDetpartments()).Returns(new List<Department>());

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.GetEmployee(Guid.NewGuid());

            //Assert
            Assert.IsNull(employee);
        }

    }
}
