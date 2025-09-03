using HRMS.Domain.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Projects
{
    public interface IProjectRepository
    {
        Task CreateProjectAsync(ProjectsModel project);
        Task<ProjectsModel> GetByIdAsync(Guid projectId);
        Task<List<ProjectsModel>> GetAllProjectsByCompanyIdAsync(Guid comId);
        Task<List<ProjectsModel>> GetAllProjectsByDivisionIdAsync(Guid divId);
        Task<List<ProjectsModel>> GetAllProjectsByOrganizationIdAsync(Guid orgId);
        Task UpdateProjectAsync(ProjectsModel project);
        Task<bool> DeleteProjectAsync(Guid projectId);
    }
}
