using HRMS.API.Responses;
using HRMS.Application.Commands.HelpDesk;
using HRMS.Application.Queries.Divisions;
using HRMS.Application.Queries.HelpDesk;
using HRMS.Domain.Entities.Divisions;
using HRMS.Domain.Entities.HelpDesk;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.HelpDesk
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpdeskTicketController : Controller
    {
        private readonly IMediator _mediator;

        public HelpdeskTicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateHelpdeskTicketCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateTicket), new StatusResponse<object>(true, "Ticket created successfully", new { id }));

        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _mediator.Send(new GetHelpdeskTicketByIdQuery(Id));
            if (result == null)
                return NotFound(new StatusResponse<HelpdeskTicketModel>(false, "Ticket not found"));

            return Ok(new StatusResponse<HelpdeskTicketModel>(true, "Ticket fetched successfully", result));
        }

        [HttpPost("AllTicketsByEmpId{empId}")]
        public async Task<IActionResult> GetAllTicketsByEmpId(Guid empId)
        {
            var result = await _mediator.Send(new GetAllHelpdeskTicketByEmpIdQuery(empId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<HelpdeskTicketModel>>(false, "No tickets found for the employee"));
            return Ok(new StatusResponse<List<HelpdeskTicketModel>>(true, "Tickets fetched successfully", result));
        }

        [HttpPost("AllTicketsByOrgId{orgId}")]
        public async Task<IActionResult> GetAllTicketsByOrgId(Guid orgId)
        {
            var result = await _mediator.Send(new GetAllHelpdeskTicketByOrgIdQuery(orgId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<HelpdeskTicketModel>>(false, "No tickets found for the organization"));
            return Ok(new StatusResponse<List<HelpdeskTicketModel>>(true, "Tickets fetched successfully", result));
        }

        [HttpPut("update/{Id}")]
        public async Task<IActionResult> UpdateTicket(Guid Id, [FromBody] UpdateHelpdeskTicketCommand command)
        {
            if (Id == Guid.Empty || command == null || Id != command.Id)
            {
                return BadRequest("Invalid ticket data.");
            }
            await _mediator.Send(command);
            return Ok(new StatusResponse<object>(true, "Ticket updated successfully", null));
        }
        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteTicket(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Invalid ticket ID.");
            }
            var deleted = await _mediator.Send(new DeleteHelpdeskTicketCommand(Id));
            if (!deleted)
                return NotFound(new StatusResponse<object>(false, "Ticket not found"));
            return Ok(new StatusResponse<object>(true, "Ticket deleted successfully"));
        }
    }
}
