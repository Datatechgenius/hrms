using HRMS.API.Responses;
using HRMS.Application.Commands.Projects;
using HRMS.Application.Commands.Roles;
using HRMS.Application.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Roles
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;
        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid role data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateRole), new StatusResponse<object>(true, "Role created successfully", new { id }));
        }

        [HttpGet("get/{roleId}")]
        public async Task<IActionResult> GetRoleById(Guid roleId)
        {
            var result = await _mediator.Send(new GetRoleByIdQuery(roleId));
            if (result == null)
            {
                return NotFound(new StatusResponse<object>(false, "Role not found"));
            }
            return Ok(new StatusResponse<object>(true, "Role retrieved successfully", result));
        }

        [HttpPost("getallbyorganization/{organizationId}")]
        public async Task<IActionResult> GetAllRolesByOrganization(Guid organizationId)
        {
            if (organizationId == Guid.Empty)
            {
                return BadRequest("Invalid organization ID.");
            }
            var roles = await _mediator.Send(new GetAllRolesByOrganizationQuery(organizationId));
            return Ok(new StatusResponse<object>(true, "Roles retrieved successfully", roles));
        }

        [HttpPut("update/{roleId}")]
        public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleCommand command)
        {
            if (roleId != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched Role ID"));
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Role updated successfully"));
        }

        [HttpDelete("delete/{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var command = new DeleteProjectCommand(roleId);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new StatusResponse<object>(true, "Role deleted successfully"));
            }
            return NotFound(new StatusResponse<object>(false, "Role not found"));
        }
    }
}