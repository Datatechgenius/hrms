using HRMS.Application.Queries.HelpDesk;
using HRMS.Domain.Entities.HelpDesk;
using HRMS.Infrastructure.Interfaces.HelpDesk;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.HelpDesk
{
    public class GetAllHelpdeskTicketByEmpIdQueryHandler : IRequestHandler<GetAllHelpdeskTicketByEmpIdQuery, List<HelpdeskTicketModel>>
    {
        private readonly IHelpdeskTicketRepository _helpdeskTicketRepository;
        public GetAllHelpdeskTicketByEmpIdQueryHandler(IHelpdeskTicketRepository helpdeskTicketRepository)
        {
            _helpdeskTicketRepository = helpdeskTicketRepository;
        }
        public async Task<List<HelpdeskTicketModel>> Handle(GetAllHelpdeskTicketByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _helpdeskTicketRepository.GetAllHelpdeskTicketsByEmpIdAsync(request.EmpId);
            return tickets;
        }
    }
}
