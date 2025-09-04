using HRMS.Application.Queries.HelpDeskComments;
using HRMS.Domain.Entities.HelpDeskComments;
using HRMS.Infrastructure.Interfaces.HelpDeskComments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.HelpDeskComments
{
    public class GetAllCommentsByTicketIdQueryHandler : IRequestHandler<GetAllCommentsByTicketIdQuery, List<HelpDeskCommentsModel>>
    {
        private readonly IHelpDeskCommentsRepository _helpDeskCommentsRepository;
        public GetAllCommentsByTicketIdQueryHandler(IHelpDeskCommentsRepository helpDeskCommentsRepository)
        {
            _helpDeskCommentsRepository = helpDeskCommentsRepository;
        }

        public async Task<List<HelpDeskCommentsModel>> Handle(GetAllCommentsByTicketIdQuery request, CancellationToken cancellationToken)
        {
            return await _helpDeskCommentsRepository.GetAllHelpdeskCommentsByTicketIdAsync(request.TicketId);
        }
    }
}
