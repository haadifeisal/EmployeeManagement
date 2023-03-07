using AutoMapper;
using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using EmployeeManagement.WebApi.DataTransferObjects.ResponseDtos;
using EmployeeManagement.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IMapper mapper, IDepartmentService departmentService)
        {
            _mapper = mapper;
            _departmentService = departmentService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<DepartmentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetDepartments();

            if (!departments.Any())
            {
                return NoContent();
            }

            var mappedResult = _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);

            return Ok(mappedResult);
        }

        [HttpGet("{departmentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDepartment([FromRoute] Guid departmentId)
        {
            var department = await _departmentService.GetDepartment(departmentId);

            if (department == null)
            {
                return NotFound();
            }

            var mappedResult = _mapper.Map<DepartmentResponseDto>(department);

            return Ok(mappedResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDeployment([FromBody] DepartmentRequestDto departmentRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _departmentService.CreateDepartment(departmentRequestDto);

            if (department == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<DepartmentResponseDto>(department);

            return Ok(mappedResult);
        }

        [HttpPut("{departmentId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(DepartmentResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid departmentId, [FromBody] DepartmentRequestDto departmentRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _departmentService.UpdateDepartment(departmentId, departmentRequestDto);

            if (department == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<DepartmentResponseDto>(department);

            return Ok(mappedResult);
        }

        [HttpDelete("{departmentId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid departmentId)
        {
            var departmentDeleted = await _departmentService.DeleteDepartment(departmentId);

            if (!departmentDeleted)
            {
                return NotFound();
            }

            return Ok(departmentDeleted);
        }

    }
}
