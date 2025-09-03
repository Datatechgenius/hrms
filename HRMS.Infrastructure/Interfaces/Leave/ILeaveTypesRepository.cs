using HRMS.Domain.Entities.Leave;
using HRMS.Domain.Entities.ProjectAssigment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Leave
{
    public interface ILeaveTypesRepository
    {
        Task InsertAsync(LeaveTypesModel leaveType);
        Task<LeaveTypesModel> GetByIdAsync(Guid id);
        Task<List<LeaveTypesModel>> GetAllAsync(Guid Id);
        Task UpdateAsync(LeaveTypesModel leaveType);
        Task<bool> DeleteAsync(Guid id);
    }
}
