using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.Configuration;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Test.UnitTests
{
    public class UnitTestBase
    {
        public readonly EmployeeManagementContext _employeeManagementcontext;
        public readonly IMapper _mapper;

        public UnitTestBase()
        {

            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<EmployeeManagementContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

            _employeeManagementcontext = new EmployeeManagementContext(options);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapConfiguration());
            });
            IMapper mapper = mappingConfig.CreateMapper();

            _mapper = mapper;

        }
    }
}
