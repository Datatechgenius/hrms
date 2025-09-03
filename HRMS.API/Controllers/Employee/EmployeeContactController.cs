using HRMS.API.Responses;
using HRMS.Application.Commands.Employee;
using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Employee;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeContactsCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "EmployeeContact created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpGet("EmployeeContact/{id}")]
        public async Task<ActionResult<StatusResponse<EmployeeContacts>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeContactByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<EmployeeContacts>(false, "EmployeeContact not found"));

            return Ok(new StatusResponse<EmployeeContacts>(true, "EmployeeContact fetched successfully", result));
        }

        [HttpPost("AllEmployeeContact/{id}")]
        public async Task<ActionResult<List<EmployeeContacts>>> GetAllEmployeeContactsByOrgId(Guid id)
        {
            var result = await _mediator.Send(new GetAllContactsByEmployeeIdQuery(id));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeContacts>>(false, "No EmployeeContact found for the organization"));

            return Ok(new StatusResponse<List<EmployeeContacts>>(true, "EmployeeContact fetched successfully", result));
        }

        [HttpPut("UpdateEmployeeContact/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeContactsCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched EmployeeContact ID"));

            if (command != null)
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "EmployeeContact updated successfully"));
            }
            else
            {
                return BadRequest(new StatusResponse<object>(false, "EmployeeContact not found"));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteEmployeeContactsCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "EmployeeContact not found"));

            return Ok(new StatusResponse<object>(true, "EmployeeContact deleted successfully"));
        }
    }
}
