using HRMS.Application.Commands.HelpDesk;
using HRMS.Infrastructure.Interfaces.HelpDesk;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.HelpDesk
{
    public class DeleteHelpdeskTicketCommandHandler : IRequestHandler<DeleteHelpdeskTicketCommand, bool>
    {
        private readonly IHelpdeskTicketRepository _helpdeskTicketRepository;
        public DeleteHelpdeskTicketCommandHandler(IHelpdeskTicketRepository helpdeskTicketRepository)
        {
            _helpdeskTicketRepository = helpdeskTicketRepository;
        }
        public async Task<bool> Handle(DeleteHelpdeskTicketCommand request, CancellationToken cancellationToken)
        {
            return await _helpdeskTicketRepository.DeleteHelpdeskTicketAsync(request.Id);
        }
    }
}
