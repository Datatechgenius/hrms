using HRMS.Domain.Entities.HelpDeskComments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.HelpDeskComments
{
    public interface IHelpDeskCommentsRepository
    {
        Task CreateHelpdeskCommentAsync(HelpDeskCommentsModel commentsModel);
        Task<HelpDeskCommentsModel> GetHelpdeskCommentByIdAsync(Guid id);
        Task<List<HelpDeskCommentsModel>> GetAllHelpdeskCommentsByTicketIdAsync(Guid ticketId);
        Task<bool> DeleteHelpdeskCommentAsync(Guid id);
    }
}
