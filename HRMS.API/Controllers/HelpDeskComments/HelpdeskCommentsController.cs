using HRMS.API.Responses;
using HRMS.Application.Commands.HelpDeskComments;
using HRMS.Application.Queries.HelpDesk;
using HRMS.Application.Queries.HelpDeskComments;
using HRMS.Domain.Entities.HelpDesk;
using HRMS.Domain.Entities.HelpDeskComments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers.HelpDeskComments
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpdeskCommentsController : Controller
    {
        private readonly IMediator _mediator;
        public HelpdeskCommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateHelpDeskCommentCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new StatusResponse<object>(true, "Ticket comment created successfully", new { id }));
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetTicketCommentByIdQuery(id));
            if (result == null)
                return NotFound(new StatusResponse<HelpDeskCommentsModel>(false, "Ticket not found"));
            return Ok(new StatusResponse<HelpDeskCommentsModel>(true, "Ticket fetched successfully", result));
        }

        [HttpPost("allcommentsbyticketid/{ticketId}")]
        public async Task<IActionResult> GetAllCommentsByTicketId(Guid ticketId)
        {
            var result = await _mediator.Send(new GetAllCommentsByTicketIdQuery(ticketId));
            if (result == null || result.Count == 0)
                return NotFound(new StatusResponse<List<HelpDeskCommentsModel>>(false, "No comments found for the ticket"));
            return Ok(new StatusResponse<List<HelpDeskCommentsModel>>(true, "Comments fetched successfully", result));
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _mediator.Send(new DeleteHelpDeskCommentCommand(id));
            if (!isDeleted)
                return NotFound(new StatusResponse<object>(false, "Comment not found or could not be deleted"));
            return Ok(new StatusResponse<object>(true, "Comment deleted successfully"));
        }
    }
}
