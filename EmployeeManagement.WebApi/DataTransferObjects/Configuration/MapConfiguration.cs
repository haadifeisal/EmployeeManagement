﻿using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using EmployeeManagement.WebApi.DataTransferObjects.ResponseDtos;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;

namespace EmployeeManagement.WebApi.DataTransferObjects.Configuration
{
    public class MapConfiguration:Profile
    {

        public MapConfiguration()
        {
            CreateMap<Employee, EmployeeResponseDto>();
            CreateMap<EmployeeRequestDto, Employee>();

            CreateMap<Department, DepartmentResponseDto>()
                .ForMember(x => x.Employees, y => y.MapFrom(z => z.Employees));
            CreateMap<DepartmentRequestDto, Department>();
        }

    }
}
