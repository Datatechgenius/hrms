using HRMS.Domain.Entities.ProjectAssigment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.ProjectAssigment
{
    public interface IProjectAssignmentRepository 
    {
        Task<List<ProjectAssignmentModel>> GetAllByEmployeeIdAsync(Guid employeeId);
        Task<ProjectAssignmentModel> GetProjectAssignmentByIdAsync(Guid id);
        Task ProjectAssignmentInsertAsync(ProjectAssignmentModel project);
        Task<bool> UpdateProjectAssignmentAsync(ProjectAssignmentModel project);
        Task<bool> DeleteProjectAssignmentAsync(Guid id);
    }
}
