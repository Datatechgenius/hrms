using Microsoft.AspNetCore.Mvc;
using HRMS.Domain.Entities;
using HRMS.API.Responses;
using HRMS.Application.Commands.Employee;
using MediatR;
using HRMS.Application.Queries.Employee;

namespace HRMS.API.Controllers.Employee
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Employee created successfully", new { id}));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StatusResponse<EmployeeModel>>> GetById(Guid id)
        {
            var emp = await _mediator.Send(new GetEmployeeByIdQuery(id));
            if (emp == null)
                return NotFound(new StatusResponse<EmployeeModel>(false, "Employee not found"));

            return Ok(new StatusResponse<EmployeeModel>(true, "Employee fetched successfully", emp));
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched employee ID"));

            var data = GetById(command.Id);
                
            if (data.Result is NotFoundResult)
            {
                return BadRequest(new StatusResponse<object>(false, "Employee not found"));
            }
            else 
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "Employee updated successfully"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
           var deleted = await _mediator.Send(new DeleteEmployeeCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Employee not found"));

            return Ok(new StatusResponse<object>(true, "Employee deleted successfully"));

        }


        [HttpPost("AllEmployeeByOrg/{orgid}")]
        public async Task<ActionResult<List<EmployeeModel>>> GetAllEmployeeByOrganizationId(Guid orgid)
        {
            var result = await _mediator.Send(new GetAllEmployeeByOrgnizationQuery(orgid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeModel>>(false, "No Employee found for the organization"));

            return Ok(new StatusResponse<List<EmployeeModel>>(true, "Employee fetched successfully" , result));
        }

        [HttpPost("AllEmployeeByDiv/{divid}")]
        public async Task<ActionResult<List<EmployeeModel>>> GetAllEmployeeByDivId(Guid divid)
        {
            var result = await _mediator.Send(new GetAllEmployeeByDivisionQuery(divid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<EmployeeModel>>(false, "No Employee found for the division"));

            return Ok(new StatusResponse<List<EmployeeModel>>(true, "Employee fetched successfully", result));
        }

        [HttpPost("AllEmployeeByComp/{id}")]
        public async Task<ActionResult<List<EmployeeModel>>> GetAllEmployeeByCompanyId(Guid id)
        {
            var result = await _mediator.Send(new GetAllEmployeeByCompanyQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<List<EmployeeModel>>(false, "No Employee found for the company"));

            return Ok(new StatusResponse<List<EmployeeModel>>(true, "Employee fetched successfully" , result));
        }
    }
}