using HRMS.API.Responses;
using HRMS.Application.Commands.Company;
using HRMS.Application.Commands.Departments;
using HRMS.Application.Queries.Company;
using HRMS.Application.Queries.Departments;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Departments;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Departments
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateDepartmentCommand command)
        {
            var id = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Department created successfully", new { id }));
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<StatusResponse<DepartmentModel>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetDepartmentByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<DepartmentModel>(false, "Company not found"));

            return Ok(new StatusResponse<DepartmentModel>(true, "Company fetched successfully", result));
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDepartmentCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched Departmaent ID"));

                await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Departmaent updated successfully"));
        }
        [HttpPost("AllDepartmentByCompanyId/{comId}")]
        public async Task<ActionResult<List<DepartmentModel>>> GetAllDepartmentsByComId(Guid comId)
        {
            var result = await _mediator.Send(new GetDepartmentByCompanyIdQuery(comId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<DepartmentModel>>(false, "No Departments found for the organization"));

            return Ok(new StatusResponse<List<DepartmentModel>>(true, "Departments fetched successfully", result));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteDepartmentCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Department not found"));

            return Ok(new StatusResponse<object>(true, "Department deleted successfully"));
        }
    }
}
