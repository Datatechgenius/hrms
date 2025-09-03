using HRMS.Domain.Entities.HelpDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.HelpDesk
{
    public interface IHelpdeskTicketRepository
    {
        Task CreateHelpdeskTicketAsync(HelpdeskTicketModel model);
        Task<HelpdeskTicketModel> GetHelpdeskTicketByIdAsync(Guid id);
        Task<List<HelpdeskTicketModel>> GetAllHelpdeskTicketsByEmpIdAsync(Guid empId);
        Task<List<HelpdeskTicketModel>> GetAllHelpdeskTicketsByOrgIdAsync(Guid orgId);
        Task UpdateHelpdeskTicketAsync(HelpdeskTicketModel model);
        Task<bool> DeleteHelpdeskTicketAsync(Guid id);
    }
}
