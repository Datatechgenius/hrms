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
    public class GetHelpdeskTicketByIdQueryHandler : IRequestHandler<GetHelpdeskTicketByIdQuery, HelpdeskTicketModel>
    {
        private readonly IHelpdeskTicketRepository _helpdeskTicketRepository;
        public GetHelpdeskTicketByIdQueryHandler(IHelpdeskTicketRepository helpdeskTicketRepository)
        {
            _helpdeskTicketRepository = helpdeskTicketRepository;
        }

        public async Task<HelpdeskTicketModel> Handle(GetHelpdeskTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _helpdeskTicketRepository.GetHelpdeskTicketByIdAsync(request.Id);
            return ticket;
        }
    }
}
