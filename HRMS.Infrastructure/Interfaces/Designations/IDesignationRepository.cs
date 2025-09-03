using HRMS.Domain.Entities.Designations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Designations
{
    public interface IDesignationRepository
    {
        Task CreateDesignationAsync(DesignationModel designation);
        Task<GetDesignationModel> GetDesignationByIdAsync(Guid id);
        Task<List<GetDesignationModel>> GetDesignationByDepartmentIdAsync(Guid companyId);

        //Task<IEnumerable<Domain.Entities.Designations.DesignationModel>> GetAllDesignationsAsync();
        Task UpdateDesignationAsync(DesignationModel designation);
        Task<bool> DeleteDesignationAsync(Guid id);
    }
}
