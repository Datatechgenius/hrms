using HRMS.API.Responses;
using HRMS.Application.Commands.Employee.EmployeeProject;
using HRMS.Application.Queries.Employee.EmployeeProject;
using HRMS.Domain.Entities.Employee;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeProjectsControllers : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeProjectsControllers(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeProjectsCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "EmployeeProjects created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }
        [HttpGet("EmployeeProjects/{id}")]
        public async Task<ActionResult<StatusResponse<EmployeeProjects>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeProjectsByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<EmployeeProjects>(
                    false,
                    "EmployeeProjects not found",
                    null
                ));

            // Include the domain object in the response
            return Ok(new StatusResponse<EmployeeProjects>(
                true,
                "EmployeeProjects fetched successfully",
                result
            ));
        }

        [HttpPost("AllEmployeeProjects/{id}")]
        public async Task<ActionResult<List<EmployeeProjects>>> GetAllEmployeeProjectsByOrgId(Guid id)
        {
            var result = await _mediator.Send(new GetAllEmployeeProjectsByIdQuery(id));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeProjects>>(false, "No EmployeeProjects found for the organization"));

            return Ok(new StatusResponse<List<EmployeeProjects>>(true, "EmployeeProjects fetched successfully", result));
        }

        [HttpPut("UpdateEmployeeProjects/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeProjectsCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched EmployeeProjects ID"));

            var data = GetById(command.Id);
            if (command != null)
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "EmployeeProjects updated successfully"));
            }
            else
            {
                return BadRequest(new StatusResponse<object>(false, "EmployeeProjects not found"));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteEmployeeProjectsCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "EmployeeProjects not found"));

            return Ok(new StatusResponse<object>(true, "EmployeeProjects deleted successfully"));
        }
    }
}