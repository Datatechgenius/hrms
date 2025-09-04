using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.Commands;
using HRMS.Application.Queries.Organization;
using HRMS.Application.Commands.Organization;
using HRMS.API.Responses;

namespace HRMS.API.Controllers.Organization
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationCommand cmd)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));

            var query = new GetOrganizationByIdQuery { Id = cmd.Id};

            var organization = await _mediator.Send(query);

            if (organization == null)
            {
                var id = await _mediator.Send(cmd);
           
                    //return NotFound(new StatusResponse<object>(false, "Organization with ame fields"));
                return CreatedAtAction(nameof(Create), new { id }, new StatusResponse<Guid>(true, "Organization created successfully", id));
            }
            else
            {
                return NotFound(new StatusResponse<object>(false, "Organization with same fields"));
            }
        }

        [HttpGet("organization/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new StatusResponse<object>(false, "Invalid ID"));

            var query = new GetOrganizationByIdQuery { Id = id };
            var organization = await _mediator.Send(query);

            if (organization == null)
                return NotFound(new StatusResponse<object>(false, "Organization not found"));

            return Ok(new StatusResponse<object>(true, "Organization retrieved successfully", organization));
        }



        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrganizationCommand cmd)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));

            if (id != cmd.Id)
                return BadRequest(new StatusResponse<object>(false, "Route ID and body ID must match"));

            var success = await _mediator.Send(cmd);
            if (!success)
                return NotFound(new StatusResponse<object>(false, "Organization not found"));

            return Ok(new StatusResponse<object>(true, "Organization updated successfully"));
        }
     
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _mediator.Send(new DeleteOrganizationCommand { Id = id });
            if (!success)
                return NotFound(new StatusResponse<object>(false, "Organization not found"));

            return Ok(new StatusResponse<object>(true, "Organization deleted successfully"));
        }
    }
}