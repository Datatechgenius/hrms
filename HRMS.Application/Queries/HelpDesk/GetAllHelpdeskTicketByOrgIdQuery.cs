using HRMS.Domain.Entities.HelpDesk;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.HelpDesk
{
    public class GetAllHelpdeskTicketByOrgIdQuery : IRequest<List<HelpdeskTicketModel>>
    {
        public Guid OrgId { get; }
        public GetAllHelpdeskTicketByOrgIdQuery(Guid orgId)
        {
            OrgId = orgId;
        }
    }
}
