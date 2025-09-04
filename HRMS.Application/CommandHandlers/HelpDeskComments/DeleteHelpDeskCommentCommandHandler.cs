using HRMS.Application.Commands.HelpDeskComments;
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
    public class DeleteHelpDeskCommentCommandHandler : IRequestHandler<DeleteHelpDeskCommentCommand, bool>
    {
        private readonly IHelpDeskCommentsRepository _helpDeskCommentsRepository;
        public DeleteHelpDeskCommentCommandHandler(IHelpDeskCommentsRepository helpDeskCommentsRepository)
        {
            _helpDeskCommentsRepository = helpDeskCommentsRepository;
        }
        public async Task<bool> Handle(DeleteHelpDeskCommentCommand request, CancellationToken cancellationToken)
        {
           return await _helpDeskCommentsRepository.DeleteHelpdeskCommentAsync(request.Id);
        }
    }
}
