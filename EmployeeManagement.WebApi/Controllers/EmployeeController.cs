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
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<EmployeeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployees();

            if (!employees.Any())
            {
                return NoContent();
            }

            var mappedResult = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);

            return Ok(mappedResult);
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid employeeId)
        {
            var employee = await _employeeService.GetEmployee(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var mappedResult = _mapper.Map<EmployeeResponseDto>(employee);

            return Ok(mappedResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRequestDto employeeRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeService.CreateEmployee(employeeRequestDto);

            if (employee == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<EmployeeResponseDto>(employee);

            return Ok(mappedResult);
        }


        [HttpPut("{employeeId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(EmployeeResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid employeeId, [FromBody] EmployeeRequestDto employeeRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeService.UpdateEmployee(employeeId,employeeRequestDto);

            if (employee == null)
            {
                return UnprocessableEntity();
            }

            var mappedResult = _mapper.Map<EmployeeResponseDto>(employee);

            return Ok(mappedResult);
        }

        [HttpDelete("{employeeId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid employeeId)
        {
            var employeeDeleted = await _employeeService.DeleteEmployee(employeeId);

            if (!employeeDeleted)
            {
                return NotFound();
            }

            return Ok(employeeDeleted);
        }

    }
}
