using HRMS.API.Responses;
using HRMS.Application.Commands.PayslipHistory;
using HRMS.Application.Queries.PayslipHistory;
using HRMS.Domain.Entities.PayslipHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.PayslipHistory
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayslipHistoryController : Controller
    {
        private readonly IMediator _mediator;
        public PayslipHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayslipHistory([FromBody] CreatePayslipHistoryCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid payroll wages data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreatePayslipHistory), new StatusResponse<object>(true, "Payroll wages created successfully", new { id }));
        }

        [HttpGet("getbyid{id}")]
        public async Task<IActionResult> GetPayslipHistoryById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid payroll wages ID.");
            }
            var result = await _mediator.Send(new GetPayslipHistoryByIdQuery(id));
            return Ok(new StatusResponse<PaySlipHistoryModel>(true, "Payroll Wages fetched successfully", result));
        }
        [HttpPost("getallbyemp{empid}")]
        public async Task<ActionResult<List<PaySlipHistoryModel>>> GetAllPayslipHistoryByEmpId(Guid empid)
        {
            var result = await _mediator.Send(new GetAllPayslipHistoryByEmpIdQuery(empid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<PaySlipHistoryModel>>(false, "No Payslip History found for the Employee"));
            return Ok(new StatusResponse<List<PaySlipHistoryModel>>(true, "Payslip History fetched successfully", result));
        }
        [HttpPut("update{id}")]
        public async Task<IActionResult> UpdatePayslipHistory(Guid id, [FromBody] UpdatePayslipHistoryCommand command)
        {
            if (id == Guid.Empty || command == null || id != command.Id)
            {
                return BadRequest("Invalid payslip history data.");
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Payslip history updated successfully", null));
        }
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> DeletePayslipHistory(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid payslip history ID.");
            }
            var deleted = await _mediator.Send(new DeletePayslipHistoryCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Payslip not found"));

            return Ok(new StatusResponse<object>(true, "Payslip deleted successfully"));
        }
    }
}
