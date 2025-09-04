using HRMS.Domain.Entities.HelpDeskComments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.HelpDeskComments
{
    public class GetTicketCommentByIdQuery : IRequest<HelpDeskCommentsModel>
    {
        public Guid Id { get; set; }
        public GetTicketCommentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
