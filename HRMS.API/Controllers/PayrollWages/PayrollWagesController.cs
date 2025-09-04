using HRMS.API.Responses;
using HRMS.Application.Commands.PayrollWages;
using HRMS.Application.Queries.PayrollWages;
using HRMS.Domain.Entities.PayrollWages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.PayrollWages
{

    [ApiController]
    [Route("api/[controller]")]

    public class PayrollWagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PayrollWagesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayrollWages([FromBody] CreatePayrollWagesCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid payroll wages data.");
            }
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreatePayrollWages), new StatusResponse<object>(true, "Payroll wages created successfully", new { id }));
        }
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetPayrollWagesById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid payroll wages ID.");
            }
            var result = await _mediator.Send(new GetPayrollWagesByIdQuery(id));
            return Ok(new StatusResponse<PayrollWagesModel>(true, "Payroll Wages fetched successfully", result));
        }

        [HttpPost("allbypayroll/{payrollId}")]
        public async Task<ActionResult<List<PayrollWagesModel>>> GetAllPayrollWagesByPayrollId(Guid payrollId)
        {
            var result = await _mediator.Send(new GetPayrollWagesByPayrollIdQuery(payrollId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<PayrollWagesModel>>(false, "No Payroll Wages found for the Payroll"));
            return Ok(new StatusResponse<List<PayrollWagesModel>>(true, "Payroll Wages fetched successfully", result));
        }

        [HttpPost("allbyemployee/{empId}")]
        public async Task<ActionResult<List<PayrollWagesModel>>> GetAllPayrollWagesByEmployeeId(Guid empId)
        {
            var result = await _mediator.Send(new GetPayrollWagesByEmpIdQuery(empId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<PayrollWagesModel>>(false, "No Payroll Wages found for the Employee"));
            return Ok(new StatusResponse<List<PayrollWagesModel>>(true, "Payroll Wages fetched successfully", result));
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePayrollWages(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new StatusResponse<object>(false, "Invalid payroll wages ID."));
            }
            await _mediator.Send(new DeletePayrollWagesCommand(id));
            return Ok(new StatusResponse<object>(true, "Payroll wages deleted successfully"));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePayrollWages(Guid id, [FromBody] UpdatePayrollWagesCommand command)
        {
            if (id == Guid.Empty || command == null || id != command.Id)
            {
                return BadRequest(new StatusResponse<object>(false, "Invalid payroll wages data."));
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Payroll wages updated successfully"));
        }
    }

}
