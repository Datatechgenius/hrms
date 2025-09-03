using HRMS.Domain.Entities.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Timesheet
{
    public interface ITimesheetRepository
    {
        Task CreateTimesheetAsync(TimesheetModel timesheet);
        Task UpdateTimesheetAsync(TimesheetModel timesheetModel);
        Task<bool> DeleteTimesheetAsync(Guid timesheetId);
        Task<TimesheetModel> GetTimesheetByIdAsync(Guid timesheetId);
        Task<List<TimesheetModel>> GetAllTimesheetsByEmployeeIdAsync(Guid employeeId);
    }
}
