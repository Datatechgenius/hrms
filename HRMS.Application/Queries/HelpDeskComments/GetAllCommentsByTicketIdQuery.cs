using HRMS.Domain.Entities.HelpDeskComments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.HelpDeskComments
{
    public class GetAllCommentsByTicketIdQuery : IRequest<List<HelpDeskCommentsModel>>
    {
        public Guid TicketId { get; set; }
        public GetAllCommentsByTicketIdQuery(Guid ticketId)
        {
            TicketId = ticketId;
        }
    }
}
