using HRMS.API.Responses;
using HRMS.Application.Commands.Departments;
using HRMS.Application.Commands.Designations;
using HRMS.Application.Queries.Designations;
using HRMS.Domain.Entities.Designations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Designations
{

    [ApiController]
    [Route("api/[controller]")]
    public class DesignationsController : Controller
    {
        private readonly IMediator _mediator;
        public DesignationsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateDesignationCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Designation created successfully", new { id }));
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<StatusResponse<GetDesignationModel>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetDesignationByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<GetDesignationModel>(false, "Designation not found"));

            return Ok(new StatusResponse<GetDesignationModel>(true, "Designation fetched successfully", result));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDesignationCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "ID mismatch"));
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Designation updated successfully"));
        }

        [HttpPost("AllDesignationsByDepartmantId/{depId}")]
        public async Task<ActionResult<List<GetDesignationModel>>> GetAllDesignationsByComId(Guid depId)
        {
            var result = await _mediator.Send(new GetDesignationByDepartmentIdQuery(depId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<GetDesignationModel>>(false, "No Designation found for the Department"));

            return Ok(new StatusResponse<List<DesignationModel>>(true, "Designations fetched successfully", result));
        }
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
             var deleted = await _mediator.Send(new DeleteDesignationCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Designation not found"));

            return Ok(new StatusResponse<object>(true, "Designation deleted successfully"));
        }
    }
}       