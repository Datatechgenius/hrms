using HRMS.API.Responses;
using HRMS.Application.Commands.Divisions;
using HRMS.Application.Queries.Divisions;
using HRMS.Domain.Entities.Divisions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Divisions
{
    [ApiController]
    [Route("api/[controller]")]
    public class DivisionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DivisionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDivisionCommand command)
        {
            var id = await _mediator.Send(command);
            var response = new StatusResponse<object>(true, "Division created successfully", new { id });
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }

        [HttpGet("Division/{id}")]
        public async Task<ActionResult<StatusResponse<DivisionResponseDto>>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetDivisionByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<DivisionResponseDto>(false, "Division not found"));

            return Ok(new StatusResponse<DivisionResponseDto>(true, "Division fetched successfully", result));
        }

        [HttpPost("AllDivisions/{orgid}")]
        public async Task<ActionResult<List<DivisionResponseDto>>> GetAllDivisionByOrgId(Guid orgid)
        {
            var result = await _mediator.Send(new GetAllDivisionByIdQuery(orgid));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<DivisionResponseDto>>(false, "No divisions found for the organization"));

            return Ok(new StatusResponse<List<DivisionResponseDto>>(true, "Divisions fetched successfully", result));
        }


        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDivisionCommand command)
        {
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "Mismatched division ID"));

            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Division updated successfully"));
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _mediator.Send(new DeleteDivisionCommand(id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Division not found"));

            return Ok(new StatusResponse<object>(true, "Division deleted successfully"));
        }
    }
}
