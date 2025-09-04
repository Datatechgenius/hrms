using HRMS.API.Responses;
using HRMS.Application.Commands.Location;
using HRMS.Application.Queries.Location;
using HRMS.Application.QueryHandlers.Location;
using HRMS.Domain.Entities.Location;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.Location
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLocationCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id }, new StatusResponse<Guid>(true, "Location created successfully", id));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var query = new GetLocationByIdQuery(Id);
            var location = await _mediator.Send(query);
            if (location == null)
                return NotFound(new StatusResponse<object>(false, "Location not found"));
            return Ok(new StatusResponse<LocationModel>(true, "Location retrieved successfully", location));
        }

        [HttpPost("AllLocationsByOrgId/{orgId}")]
        public async Task<ActionResult<List<LocationModel>>> GetAllLocationsByOrgId(Guid orgId)
        {
            var query = new GetAllLocationsByOrgIdQuery(orgId);
            var locations = await _mediator.Send(query);
            if (locations == null || locations.Count == 0)
                return NotFound(new StatusResponse<List<LocationModel>>(false, "No locations found for the organization"));
            return Ok(new StatusResponse<List<LocationModel>>(true, "Locations retrieved successfully", locations));
        }
        [HttpPost("AllLocationsByDivId/{divId}")]
        public async Task<ActionResult<List<LocationModel>>> GetAllLocationsByDivId(Guid divId)
        {
            var query = new GetAllLocationsByDivIdQuery(divId);
            var locations = await _mediator.Send(query);
            if (locations == null || locations.Count == 0)
                return NotFound(new StatusResponse<List<LocationModel>>(false, "No locations found for the division"));
            return Ok(new StatusResponse<List<LocationModel>>(true, "Locations retrieved successfully", locations));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLocationCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(new StatusResponse<object>(false, "Validation failed", ModelState));
            if (id != command.Id)
                return BadRequest(new StatusResponse<object>(false, "ID mismatch"));
            try
            {
                await _mediator.Send(command);
                return Ok(new StatusResponse<object>(true, "Location updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new StatusResponse<object>(false, "Location not found"));
            }
            catch (Exception ex)
            {
                return BadRequest(new StatusResponse<object>(false, "An error occurred while updating the location", ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteLocationCommand(id));
                if (!result)
                    return NotFound(new StatusResponse<object>(false, "Location not found"));
                return Ok(new StatusResponse<object>(true, "Location deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new StatusResponse<object>(false, "An error occurred while deleting the location", ex.Message));
            }
        }
    }
}
