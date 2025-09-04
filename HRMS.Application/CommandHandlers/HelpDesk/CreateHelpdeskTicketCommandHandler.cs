using HRMS.Application.Commands.HelpDesk;
using HRMS.Domain.Entities.HelpDesk;
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
    public class CreateHelpdeskTicketCommandHandler : IRequestHandler<CreateHelpdeskTicketCommand, Guid>
    {
        private readonly IHelpdeskTicketRepository _helpdeskTicketRepository;
        public CreateHelpdeskTicketCommandHandler(IHelpdeskTicketRepository helpdeskTicketRepository)
        {
            _helpdeskTicketRepository = helpdeskTicketRepository;
        }
        public async Task<Guid> Handle(CreateHelpdeskTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketdata = new HelpdeskTicketModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                EmployeeId = request.EmployeeId,
                Subject = request.Subject,
                Description = request.Description,
                Priority = request.Priority,
                Status = request.Status,
                AssignedTo = request.AssignedTo,
                Resolution = request.Resolution,
                ResolutionDate = request.ResolutionDate,
                SlaDueDate = request.SlaDueDate,
                Channel = request.Channel,
                AttachmentsUrl = request.AttachmentsUrl,
                CreatedAt = DateTime.UtcNow
            };
            await _helpdeskTicketRepository.CreateHelpdeskTicketAsync(ticketdata);
            return ticketdata.Id;
        }
    }
}
