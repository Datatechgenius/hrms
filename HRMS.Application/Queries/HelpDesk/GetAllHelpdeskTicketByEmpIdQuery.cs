using HRMS.Domain.Entities.HelpDesk;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.HelpDesk
{
    public class GetAllHelpdeskTicketByEmpIdQuery : IRequest<List<HelpdeskTicketModel>>
    {
        public Guid EmpId { get; }
        public GetAllHelpdeskTicketByEmpIdQuery(Guid empId)
        {
            EmpId = empId;
        }
    }
}
