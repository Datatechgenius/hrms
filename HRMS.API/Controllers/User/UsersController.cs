using HRMS.API.Responses;
using HRMS.Application.Commands;
using HRMS.Application.Commands.User;
using HRMS.Application.Queries.User;
using HRMS.Application.Query.User;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid user data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateUser), new StatusResponse<object>(true, "User created successfully", new { id }));
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId));
            if (result == null)
            {
                return NotFound(new StatusResponse<object>(false, "User not found"));
            }
            return Ok(new StatusResponse<object>(true, "User retrieved successfully", result));
        }

        [HttpGet("getAll/{orgId}")]
        public async Task<IActionResult> GetAllUsers(Guid orgId)
        {
            var result = await _mediator.Send(new GetAllUsersQueryByOrg(orgId));
            if (result == null || !result.Any())
            {
                return NotFound(new StatusResponse<object>(false, "No users found"));
            }
            return Ok(new StatusResponse<object>(true, "Users retrieved successfully", result));
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserCommand command)
        {
            if (userId != command.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "User ID mismatch"));
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "User updated successfully"));
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand(userId));
            if (!result)
            {
                return NotFound(new StatusResponse<object>(false, "User not found or could not be deleted"));
            }
            return Ok(new StatusResponse<object>(true, "User deleted successfully"));
        }
    }
}
