using HRMS.API.Responses;
using HRMS.Application.Commands.Employee;
using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeFamilyMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeFamilyMemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeFamilyMemberCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "EmployeeFamilyMember created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpGet("EmployeeFamilyMember/{id}")]
        public async Task<ActionResult<StatusResponse<EmployeeFamilyMembers>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeFamilyMembersByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<EmployeeFamilyMembers>(false, "EmployeeFamilyMember not found"));

            return Ok(new StatusResponse<EmployeeFamilyMembers>(true, "EmployeeFamilyMember fetched successfully", result));
        }

        [HttpPost("AllEmployeeFamilyMember/{id}")]
        public async Task<ActionResult<List<EmployeeFamilyMembers>>> GetAllEmployeeFamilyMembersByOrgId(Guid id)
        {
            var result = await _mediator.Send(new GetAllEmployeeFamilyMembersByIdQuery(id));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeFamilyMembers>>(false, "No EmployeeFamilyMember found for the organization"));

            return Ok(new StatusResponse<List<EmployeeFamilyMembers>>(true, "EmployeeFamilyMember fetched successfully" , result));
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeFamilyMembersCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched EmployeeFamilyMembers ID"));

            var data = GetById(command.Id);
            if (command != null)
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "EmployeeFamilyMembers updated successfully"));
            }
            else
            {
                return BadRequest(new StatusResponse<object>(false, "EmployeeFamilyMembers not found"));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteEmployeeFamilyMembersCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "EmployeeFamilyMember not found"));

            return Ok(new StatusResponse<object>(true, "EmployeeFamilyMember deleted successfully"));
        }
    }
}