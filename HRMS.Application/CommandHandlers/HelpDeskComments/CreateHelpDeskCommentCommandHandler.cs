using HRMS.Application.Commands.HelpDeskComments;
using HRMS.Domain.Entities.HelpDeskComments;
using HRMS.Infrastructure.Interfaces.HelpDeskComments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.HelpDeskComments
{
    public class CreateHelpDeskCommentCommandHandler : IRequestHandler<CreateHelpDeskCommentCommand, Guid>
    {
        private readonly IHelpDeskCommentsRepository _helpDeskCommentsRepository;

        public CreateHelpDeskCommentCommandHandler(IHelpDeskCommentsRepository helpDeskCommentsRepository)
        {
            _helpDeskCommentsRepository = helpDeskCommentsRepository;
        }
        public async Task<Guid> Handle(CreateHelpDeskCommentCommand request, CancellationToken cancellationToken)
        {
            var newComment = new HelpDeskCommentsModel
            {
                Id = Guid.NewGuid(),
                TicketId = request.TicketId,
                EmployeeId = request.EmployeeId,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };
            await _helpDeskCommentsRepository.CreateHelpdeskCommentAsync(newComment);
            return newComment.Id;
        }
    }
}
