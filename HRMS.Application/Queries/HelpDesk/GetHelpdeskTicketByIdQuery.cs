using HRMS.Domain.Entities.HelpDesk;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.HelpDesk
{
    public class GetHelpdeskTicketByIdQuery : IRequest<HelpdeskTicketModel>
    {
        public Guid Id { get; }
        public GetHelpdeskTicketByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
