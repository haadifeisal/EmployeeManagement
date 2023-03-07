using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FakeItEasy;
using EmployeeManagement.WebApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.WebApi.Test.UnitTests
{
    [TestClass]
    public class EmployeeServiceUnitTest : UnitTestBase
    {
        [TestMethod]
        public async Task GetEmployees_WhenEmployeesExists_ShouldReturn2EmployeeObjects()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee1 = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee1);

            var newEmployee2 = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Adam",
                Salary = 47000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee2);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employees = await employeeService.GetEmployees();

            //Assert
            Assert.AreEqual(2, employees.Count());
        }

        [TestMethod]
        public async Task GetEmployees_WhenNoEmployeeIsFound_ShouldReturnEmptyCollection()
        {
            //Arrange
            var departmentServiceMock = A.Fake<IDepartmentService>();

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employees = await employeeService.GetEmployees();

            //Assert
            Assert.IsFalse(employees.Any());
        }

        [TestMethod]
        public async Task GetEmployee_WhenEmployeeIsFound_ShouldReturnEmployee()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

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
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.GetEmployee(Guid.NewGuid());

            //Assert
            Assert.IsNull(employee);
        }

        [TestMethod]
        public async Task CreateEmployee_WhenEmployeeIsCreated_ShouldReturnTheCreatedEmployeeObject()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);
            _employeeManagementcontext.SaveChanges();

            var employeeRequestDto = new EmployeeRequestDto
            {
                DepartmentId = newDepartment.DepartmentId,
                Name = "Jacob",
                Salary = 75000
            };

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).Returns(new Department());

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.CreateEmployee(employeeRequestDto);

            //Assert
            Assert.IsNotNull(employee);
            Assert.AreEqual(employeeRequestDto.Name, employee.Name);
            Assert.AreEqual(employeeRequestDto.DepartmentId, employee.DepartmentId);
            Assert.IsNotNull(_employeeManagementcontext.Employees.AsNoTracking()
                .FirstOrDefault(x => x.EmployeeId == employee.EmployeeId));
            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task CreateEmployee_WhenDepartmentIsNotFound_ShouldReturnNull()
        {
            //Arrange
            var employeeRequestDto = new EmployeeRequestDto
            {
                DepartmentId = Guid.NewGuid(),
                Name = "Jacob",
                Salary = 75000
            };

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(employeeRequestDto.DepartmentId)).Returns(Task.FromResult<Department>(null));

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.CreateEmployee(employeeRequestDto);

            //Assert
            Assert.IsNull(employee);
            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(employeeRequestDto.DepartmentId)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task UpdateEmployee_WhenEmployeeIsUpdated_ShouldReturnTheUpdatedEmployeeObject()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var employeeRequestDto = new EmployeeRequestDto
            {
                DepartmentId = newDepartment.DepartmentId,
                Name = "Mike",
                Salary = 49000
            };

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).Returns(new Department());

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.UpdateEmployee(newEmployee.EmployeeId, employeeRequestDto);

            //Assert
            Assert.IsNotNull(employee);
            Assert.AreEqual(employeeRequestDto.Name, employee.Name);
            Assert.AreEqual(employeeRequestDto.Salary, employee.Salary);
            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task UpdateEmployee_WhenEmployeeIsNotFound_ShouldReturnNull()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var employeeRequestDto = new EmployeeRequestDto
            {
                DepartmentId = newDepartment.DepartmentId,
                Name = "Mike",
                Salary = 49000
            };

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).Returns(Task.FromResult<Department>(null));

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.UpdateEmployee(Guid.NewGuid(), employeeRequestDto);

            //Assert
            Assert.IsNull(employee);
            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).MustNotHaveHappened(); // This call can not be made because we return null before we execute CheckIfDepartmentExist.
        }

        [TestMethod]
        public async Task UpdateEmployee_WhenDepartmentIsNotFound_ShouldReturnNull()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var employeeRequestDto = new EmployeeRequestDto
            {
                DepartmentId = newDepartment.DepartmentId,
                Name = "Mike",
                Salary = 49000
            };

            var departmentServiceMock = A.Fake<IDepartmentService>();

            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).Returns(Task.FromResult<Department>(null));

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.UpdateEmployee(newEmployee.EmployeeId, employeeRequestDto);

            //Assert
            Assert.IsNull(employee);
            A.CallTo(() => departmentServiceMock.CheckIfDepartmentExist(newDepartment.DepartmentId)).MustHaveHappenedOnceExactly();
        }

        [TestMethod]
        public async Task DeleteEmployee_WhenEmployeeIsDeleted_ShouldReturnTrue()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.DeleteEmployee(newEmployee.EmployeeId);

            //Assert
            Assert.IsTrue(employee);
            Assert.IsNull(_employeeManagementcontext.Employees.AsNoTracking()
                .FirstOrDefault(x => x.EmployeeId == newEmployee.EmployeeId));
        }

        [TestMethod]
        public async Task DeleteEmployee_WhenEmployeeIsNotFound_ShouldReturnFalse()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var newEmployee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = "Hanna",
                Salary = 55000,
                DepartmentId = newDepartment.DepartmentId
            };
            _employeeManagementcontext.Employees.Add(newEmployee);
            _employeeManagementcontext.SaveChanges();

            var departmentServiceMock = A.Fake<IDepartmentService>();

            var employeeService = new EmployeeService(_mapper, _employeeManagementcontext, departmentServiceMock);

            //Act
            var employee = await employeeService.DeleteEmployee(Guid.NewGuid());

            //Assert
            Assert.IsFalse(employee);
        }

    }
}
