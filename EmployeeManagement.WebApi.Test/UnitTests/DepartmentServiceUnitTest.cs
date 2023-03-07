using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using EmployeeManagement.WebApi.Services.Interfaces;
using EmployeeManagement.WebApi.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.WebApi.Test.UnitTests
{
    [TestClass]
    public class DepartmentServiceUnitTest : UnitTestBase
    {
        [TestMethod]
        public async Task GetDepartments_WhenDepartmentExists_ShouldReturn3DepartmentObjects()
        {
            //Arrange
            var department1 = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(department1);

            var department2 = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "Sales"
            };
            _employeeManagementcontext.Departments.Add(department2);

            var department3 = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "Finance"
            };
            _employeeManagementcontext.Departments.Add(department3);
            _employeeManagementcontext.SaveChanges();

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var departments = await departmentService.GetDepartments();

            //Assert
            Assert.IsTrue(departments.Any());
            Assert.AreEqual(3, departments.Count());
        }

        [TestMethod]
        public async Task GetDepartments_WhenNoDepartmentIsFound_ShouldReturnEmptyCollection()
        {
            //Arrange
            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var departments = await departmentService.GetDepartments();

            //Assert
            Assert.IsFalse(departments.Any());
        }

        [TestMethod]
        public async Task GetDepartment_WhenDepartmentIsFound_ShouldReturnDepartment()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);
            _employeeManagementcontext.SaveChanges();

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.GetDepartment(newDepartment.DepartmentId);

            //Assert
            Assert.IsNotNull(department);
            Assert.AreEqual(department.DepartmentId, newDepartment.DepartmentId);
        }

        [TestMethod]
        public async Task GetDepartment_WhenDepartmentIsNotFound_ShouldReturnNull()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);
            _employeeManagementcontext.SaveChanges();

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.GetDepartment(Guid.NewGuid());

            //Assert
            Assert.IsNull(department);
        }

        [TestMethod]
        public async Task CreateDepartment_WhenDepartmentIsCreated_ShouldReturnTheCreatedDepartmentObject()
        {
            //Arrange
            var departmentRequestDto = new DepartmentRequestDto
            {
                Name = "IT"
            };

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.CreateDepartment(departmentRequestDto);

            //Assert
            Assert.IsNotNull(department);
            Assert.AreEqual(departmentRequestDto.Name, department.Name);
        }

        [TestMethod]
        public async Task UpdateDepartment_WhenDepartmentIsUpdated_ShouldReturnTheUpdatedDepartmentObject()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);
            _employeeManagementcontext.SaveChanges();

            var departmentRequestDto = new DepartmentRequestDto
            {
                Name = "Finance"
            };

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.UpdateDepartment(newDepartment.DepartmentId, departmentRequestDto);

            //Assert
            Assert.IsNotNull(department);
            Assert.AreEqual(departmentRequestDto.Name, department.Name);
        }

        [TestMethod]
        public async Task UpdateDepartment_WhenDepartmentIsNotFound_ShouldReturnNull()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);
            _employeeManagementcontext.SaveChanges();

            var departmentRequestDto = new DepartmentRequestDto
            {
                Name = "Finance"
            };

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.UpdateDepartment(Guid.NewGuid(), departmentRequestDto);

            //Assert
            Assert.IsNull(department);
        }

        [TestMethod]
        public async Task DeleteDepartment_WhenDepartmentIsDeleted_ShouldReturnTrue()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);
            _employeeManagementcontext.SaveChanges();

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.DeleteDepartment(newDepartment.DepartmentId);

            //Assert
            Assert.IsTrue(department);
            Assert.IsNull(_employeeManagementcontext.Departments.AsNoTracking()
                .FirstOrDefault(x => x.DepartmentId == newDepartment.DepartmentId));
        }

        [TestMethod]
        public async Task DeleteDepartment_WhenDepartmentIsNotFound_ShouldReturnFalse()
        {
            //Arrange
            var newDepartment = new Department
            {
                DepartmentId = Guid.NewGuid(),
                Name = "IT"
            };
            _employeeManagementcontext.Departments.Add(newDepartment);

            var departmentService = new DepartmentService(_mapper, _employeeManagementcontext);

            //Act
            var department = await departmentService.DeleteDepartment(Guid.NewGuid());

            //Assert
            Assert.IsFalse(department);
        }


    }
}
