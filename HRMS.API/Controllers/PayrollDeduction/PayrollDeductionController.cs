using HRMS.API.Responses;
using HRMS.Application.Commands.PayrollDeduction;
using HRMS.Application.Queries.PayrollDeduction;
using HRMS.Application.Queries.PayrollWages;
using HRMS.Domain.Entities.PayrollDeduction;
using HRMS.Domain.Entities.PayrollWages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.PayrollDeduction
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollDeductionController : Controller
    {
        private readonly IMediator _mediator;

        public PayrollDeductionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayrollDeduction([FromBody] CreatePayrollDeductionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid payroll data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreatePayrollDeduction), new StatusResponse<object>(true, "Payroll Deduction created successfully", new { id }));

        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetPayrollDeductionById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid payroll wages ID.");
            }
            var result = await _mediator.Send(new GetPayrollDeductionByIdQuery(id));
            return Ok(new StatusResponse<PayrollDeductionModel>(true, "Payroll Wages fetched successfully", result));
        }

        [HttpPost("getallbypayrollid/{payrollid}")]
        public async Task<IActionResult> GetAllPayrollDeductionByPayrollId(Guid payrollid)
        {
            if (payrollid == Guid.Empty)
            {
                throw new ArgumentException("Invalid payroll ID.");
            }
            var result = await _mediator.Send(new GetAllPayrollDeductionByPayrollIdQuery(payrollid));
            return Ok(new StatusResponse<List<PayrollDeductionModel>>(true, "Payroll Deductions fetched successfully", result));
        }

        [HttpPut("update/{Id}")]
        public async Task<IActionResult> UpdatePayrollDeduction(Guid Id , [FromBody] UpdatePayrollDeductionCommand command)
        {
            if (Id == null || command == null || Id != command.Id)
            {
                return BadRequest("Invalid payroll data or Id Mismatch");
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Payroll Deduction updated successfully", null));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePayrollDeduction(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid payroll deduction ID.");
            }
           var result = await _mediator.Send(new DeletePayrollDeductionCommand(id));
            if (!result)
                    return NotFound(new StatusResponse<object>(false, "Payroll Deduction not found"));
            return Ok(new StatusResponse<object>(true, "Payroll Deduction deleted successfully", null));
        }
    }
}
