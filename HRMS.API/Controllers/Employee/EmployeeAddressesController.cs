using HRMS.API.Responses;
using HRMS.Application.Commands.Employee;
using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAddressesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeAddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeAddressesCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "EmployeeAddresses created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpGet("EmployeeAddresses/{id}")]
        public async Task<ActionResult<StatusResponse<EmployeeAddresses>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeAddressesByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<EmployeeAddresses>(false, "EmployeeAddresses not found" , result));

            return Ok(new StatusResponse<EmployeeAddresses>(true, "EmployeeAddresses fetched successfully" , result));
        }

        [HttpPut("UpdateEmployeeAddresses/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeAddressesCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched EmployeeAddresses ID"));

            var data = GetById(command.Id);
            if (data.Result is NotFoundResult)
            {
                return BadRequest(new StatusResponse<object>(false, "EmployeeAddresses not found"));
            }
            else 
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "EmployeeAddresses updated successfully"));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteEmployeeAddressesCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "EmployeeAddresses not found"));

            return Ok(new StatusResponse<object>(true, "EmployeeAddresses deleted successfully"));
        }

        [HttpGet("Employee/{employeeId}/Addresses")]
        public async Task<ActionResult<StatusResponse<List<EmployeeAddresses>>>> GetAddressesByEmployeeId(Guid employeeId)
        {
            var result = await _mediator.Send(new GetAllAddressesByEmployeeIdQuery(employeeId));
    
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeAddresses>>(false, "No addresses found for this employee", result));

            return Ok(new StatusResponse<List<EmployeeAddresses>>(true, "Employee addresses fetched successfully", result));
        }
    }
}