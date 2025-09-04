using System;
using System.Threading.Tasks;
using HRMS.Domain.Entities;

namespace HRMS.Infrastructure.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Guid> OrganizationInsertAsync(OrganizationModel org);
        Task<OrganizationModel> GetByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(OrganizationModel org);
    }
}