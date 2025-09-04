using HRMS.API.Responses;
using HRMS.Application.Commands.Projects;
using HRMS.Application.Commands.RolePermission;
using HRMS.Application.Queries.RolePermission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.RolePermission
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolePermissionController : Controller
    {
        private readonly IMediator _mediator;
        public RolePermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRolePermission([FromBody] CreateRolePermissionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid role permission data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateRolePermission), new StatusResponse<object>(true, "Role permission created successfully", new { id }));
        }

        [HttpGet("get/{rolePermissionId}")]
        public async Task<IActionResult> GetRolePermissionById(Guid rolePermissionId)
        {
            var result = await _mediator.Send(new GetRolePermissionByIdQuery(rolePermissionId));
            if (result == null)
            {
                return NotFound(new StatusResponse<object>(false, "Role permission not found"));
            }
            return Ok(new StatusResponse<object>(true, "Role permission retrieved successfully", result));
        }

        [HttpPost("getAllPermissionsByRoleId/{roleId}")]
        public async Task<IActionResult> GetPermissionsByRoleId(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                return BadRequest("Invalid role ID.");
            }
            var permissions = await _mediator.Send(new GetPermissionsByRoleIdQuery(roleId));
            if (permissions == null || !permissions.Any())
            {
                return NotFound(new StatusResponse<object>(false, "No permissions found for the specified role"));
            }
            return Ok(new StatusResponse<object>(true, "Permissions retrieved successfully", permissions));
        }
        [HttpPut("update/{rolePermissionId}")]
        public async Task<IActionResult> UpdateRolePermission(Guid rolePermissionId, [FromBody] UpdateRolePermissionCommand command)
        {
            if (rolePermissionId != command.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "Mismatched Role Permission ID"));
            }
            if (command == null)
            {
                return BadRequest("Invalid role permission data.");
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Role permission updated successfully"));
        }

        [HttpDelete("delete/{rolePermissionId}")]
        public async Task<IActionResult> DeleteRolePermission(Guid rolePermissionId)
        {
             var command = new DeleteProjectCommand(rolePermissionId);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new StatusResponse<object>(true, "RolePermission deleted successfully"));
            }
            return NotFound(new StatusResponse<object>(false, "RolePermission not found"));
        }
    }
}