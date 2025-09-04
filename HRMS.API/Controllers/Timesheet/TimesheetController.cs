using HRMS.API.Responses;
using HRMS.Application.Commands;
using HRMS.Application.Commands.Timesheet;
using HRMS.Application.Queries.Timesheet;
using HRMS.Domain.Entities.Timesheet;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Timesheet
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : Controller
    {
        private readonly IMediator _mediator;
        public TimesheetController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTimesheetCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Timesheet created successfully", new { id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid timesheetId)
        {
            var result = await _mediator.Send(new GetTimesheetByIdQuery(timesheetId));
            if (result == null)
                return NotFound(new StatusResponse<TimesheetModel>(false, "Timesheet not found"));

            return Ok(new StatusResponse<TimesheetModel>(true, "Timesheet retrieved successfully", result));
        }

        [HttpPost("AllTimesheetByEmp/{empId}")]
        public async Task<ActionResult<List<TimesheetModel>>> GetAllTimesheetByEmpId(Guid empId)
        {
            var result = await _mediator.Send(new GetAllTimesheetByEmpIdQuery(empId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<TimesheetModel>>(false, "No Timesheet found for the employee"));
            return Ok(new StatusResponse<List<TimesheetModel>>(true, "Timesheets fetched successfully", result));
        }

        [HttpDelete("Delete/{timesheetId}")]
        public async Task<IActionResult> Delete(Guid timesheetId)
        {
            var result = await _mediator.Send(new DeleteTimesheetCommand(timesheetId));
            if (!result)
                return NotFound(new StatusResponse<object>(false, "Timesheet not found or could not be deleted"));

            return Ok(new StatusResponse<object>(true, "Timesheet deleted successfully"));
        }

        [HttpPut("Update/{timesheetId}")]
        public async Task<IActionResult> Update(Guid timesheetId, [FromBody] UpdateTimesheetCommand cmd)
        {
            if (timesheetId != cmd.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "Timesheet ID mismatch"));
            }
            await _mediator.Send(cmd);
            return Ok(new StatusResponse<object>(true, "Timesheet updated successfully"));
        }
    }
}