using HRMS.API.Responses;
using HRMS.Application.Commands.Employee;
using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeBankAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeBankAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeBankAccountsCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "EmployeeBankAccounts created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpGet("EmployeeBankAccounts/{id}")]
        public async Task<ActionResult<StatusResponse<EmployeeBankAccounts>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeBankAccountsByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<EmployeeBankAccounts>(false, "EmployeeBankAccounts not found"));

            return Ok(new StatusResponse<EmployeeBankAccounts>(true, "EmployeeBankAccounts fetched successfully", result));
        }

        [HttpPost("AllBankAccountsByEmployeeId/{id}")]
        public async Task<ActionResult<List<EmployeeBankAccounts>>> GetAllEmployeeBankAccountsByOrgId(Guid id)
        {
            var result = await _mediator.Send(new GetAllEmployeeBankAccountsByIdQuery(id));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeBankAccounts>>(false, "No EmployeeBankAccounts found for the organization"));

            return Ok(new StatusResponse<List<EmployeeBankAccounts>>(true, "EmployeeBankAccounts fetched successfully", result));
        }

        [HttpPut("UpdateEmployeeBankAccounts/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeBankAccountsCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched EmployeeBankAccounts ID"));

            if (command != null)
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "EmployeeBankAccounts updated successfully"));
            }
            else
            {
                return BadRequest(new StatusResponse<object>(false, "EmployeeBankAccounts not found"));
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteEmployeeBankAccountsCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "EmployeeBankAccounts not found"));

            return Ok(new StatusResponse<object>(true, "EmployeeBankAccounts deleted successfully"));
        }
    }
}
