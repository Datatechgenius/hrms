using HRMS.API.Responses;
using HRMS.Application.Commands.Leave;
using HRMS.Application.Queries.Leave;
using HRMS.Domain.Entities.Leave;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
      {
        private readonly IMediator _mediator;
        public LeaveTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveTypesCommand cmd)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));

            if (cmd != null)
            {
                var id = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Create), new { id }, new StatusResponse<Guid>(true, "LeaveTypes created successfully", id));
            }
            else
            {
                return NotFound(new StatusResponse<object>(false, "LeaveTypes creation faild"));
            }
        }

        [HttpGet("project/{leavetypesId}")]
        public async Task<IActionResult> GetById(Guid leavetypesId)
        {
            var query = new GetLeaveTypeByIdQuery(leavetypesId);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(new StatusResponse<object>(false, "LeaveTypes not found"));
            }
            return Ok(new StatusResponse<LeaveTypesModel>(true, "LeaveTypes retrieved successfully", result));
        }

        [HttpPost]
        [Route("AllLeaveTypes/{OrgId}")]
        public async Task<ActionResult<List<LeaveTypesModel>>> GetAllLeaveTypesId(Guid OrgId)
        {
            var result = await _mediator.Send(new GetAllLeaveTypesQuery(OrgId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<LeaveTypesModel>>(false, "No LeaveTypes found"));
            return Ok(new StatusResponse<List<LeaveTypesModel>>(true, "LeaveTypes fetched successfully", result));
        }

        [HttpPut]
        [Route("UpdateLeaveTypes/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLeaveTypesCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched LeaveTypes ID"));
            var existingAssignment = await _mediator.Send(new GetLeaveTypeByIdQuery(id));
            if (existingAssignment == null)
            {
                return NotFound(new StatusResponse<object>(false, "LeaveTypes not found"));
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "LeaveTypes updated successfully"));
        }

        [HttpDelete]
        [Route("DeleteLeaveTypes/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new GetLeaveTypeByIdQuery(id);
            var assignment = await _mediator.Send(query);
            if (assignment == null)
            {
                return NotFound(new StatusResponse<object>(false, "LeaveTypes not found"));
            }
            await _mediator.Send(new DeleteLeaveTypesCommand(id));
            return Ok(new StatusResponse<object>(true, "LeaveTypes deleted successfully"));
        }
    }
}