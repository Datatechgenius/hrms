using HRMS.API.Responses;
using HRMS.Application.Commands.Company;
using HRMS.Application.Commands.Payroll;
using HRMS.Application.Queries.Payroll;
using HRMS.Domain.Entities.Payroll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Payroll
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : Controller
    {
        private readonly IMediator _mediator;
        public PayrollController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayroll([FromBody] CreatePayrollCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid payroll data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreatePayroll), new StatusResponse<object>(true, "Payroll created successfully", new { id }));
        }

        [HttpGet("getbyid/{id}")]
        public async Task<PayrollModel> GetPayrollById(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ArgumentException("Invalid payroll ID.");
            }
            return await _mediator.Send(new GetPayrollByIdQuery(id));
        }
        [HttpPost("allbyorg/{orgid}")]
        public async Task<ActionResult<List<PayrollModel>>> GetAllPayrollsByOrgId(Guid orgid)
        {
            var result = await _mediator.Send(new GetPayrollsByOrgIdQuery(orgid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<PayrollModel>>(false, "No Payrolls found for the organization"));
            return Ok(new StatusResponse<List<PayrollModel>>(true, "Payrolls fetched successfully", result));
        }

        [HttpPost("allbycompany/{comid}")]
        public async Task<ActionResult<List<PayrollModel>>> GetAllPayrollsByComId(Guid comid)
        {
            var result = await _mediator.Send(new GetPayrollsByComIdQuery(comid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<PayrollModel>>(false, "No Payrolls found for the organization"));
            return Ok(new StatusResponse<List<PayrollModel>>(true, "Payrolls fetched successfully", result));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePayroll(Guid id, [FromBody] UpdatePayrollCommand command)
        {
            if (id == Guid.Empty || command == null || id != command.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "Invalid payroll data."));
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Payroll updated successfully"));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePayroll(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new StatusResponse<object>(false, "Invalid payroll ID."));
            }
            var deleted = await _mediator.Send(new DeletePayrollCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Payroll not found"));

            return Ok(new StatusResponse<object>(true, "Payroll deleted successfully"));
        }
    }
}
