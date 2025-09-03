using HRMS.API.Responses;
using HRMS.Application.Commands.Attendance;
using HRMS.Application.Queries.Attendance;
using HRMS.Domain.Entities.Attendance;
using HRMS.Domain.Entities.Company;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Attendance
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Attendance created successfully", new { id }));
        }

        [HttpGet("Get/{attendanceId}")]
        public async Task<IActionResult> GetById(Guid attendanceId)
        {
            var query = new GetAttendanceByIdQuery(attendanceId);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound(new StatusResponse<object>(false, "Attendance not found"));
            }
            return Ok(new StatusResponse<AttendanceModel>(true, "Attendance retrieved successfully", result));
        }

        [HttpPost("AllAttendanceByEmp/{empId}")]
        public async Task<ActionResult<List<AttendanceModel>>> GetAllAttendanceByEmpId(Guid empId)
        {
            var result = await _mediator.Send(new GetAllAttendanceByEmpIdQuery(empId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<AttendanceModel>>(false, "No Attendance found for the employee"));
            return Ok(new StatusResponse<List<AttendanceModel>>(true, "Attendance fetched successfully", result));
        }
        [HttpPut("Update/{attendanceId}")]
        public async Task<IActionResult> Update(Guid attendanceId, [FromBody] UpdateAttendanceCommand cmd)
        {
            if (attendanceId != cmd.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "Attendance ID mismatch"));
            }
            var result = await _mediator.Send(cmd);
            
            return Ok(new StatusResponse<object>(true, "Attendance updated successfully"));
        }

        [HttpDelete("Delete/{attendanceId}")]
        public async Task<IActionResult> Delete(Guid attendanceId)
        {
            var command = new DeleteAttendanceCommand(attendanceId);
            var result = await _mediator.Send(command);
            if (result)
            {
                return Ok(new StatusResponse<object>(true, "Attendance deleted successfully"));
            }
            return NotFound(new StatusResponse<object>(false, "Attendance not found"));
        }
    }
}
