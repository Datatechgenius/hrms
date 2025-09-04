using HRMS.API.Responses;
using HRMS.Application.Commands.Contingent;
using HRMS.Application.Queries.Contingent;
using HRMS.Domain.Entities.Contingent;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Contingent
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContingentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContingentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContingentCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "Contingent created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpGet("Contingent/{id}")]
        public async Task<ActionResult<StatusResponse<ContingentDto>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetContingentByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<ContingentDto>(false, "Contingent not found"));

            return Ok(new StatusResponse<ContingentDto>(true, "Contingent fetched successfully", result));
        }

        [HttpPost("AllContingent/{orgid}")]
        public async Task<ActionResult<List<ContingentDto>>> GetAllContingentByOrgId(Guid orgid)
        {
            var result = await _mediator.Send(new GetAllContingentByOrgIdQuery(orgid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<ContingentDto>>(false, "No Contingent found for the organization"));

            return Ok(new StatusResponse<List<ContingentDto>>(true, "Contingent fetched successfully", result));
        }


        [HttpPut("UpdateContingent/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateContingentCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched Contingent ID"));

            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Contingent updated successfully"));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteContingentCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Contingent not found"));

            return Ok(new StatusResponse<object>(true, "Contingent deleted successfully"));
        }
    }
}
