using HRMS.Domain.Entities.Contingent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Contingent
{
    public interface IContingentRepository
    {
        Task<Guid> AddContingentAsync(ContingentDto model);
        Task<ContingentDto> GetContingentByIdAsync(Guid id);
        Task<List<ContingentDto>> GetAllContingentsByOrgIdAsync(Guid Id);
        Task UpdateContingentAsync(ContingentDto model);
        Task<bool> DeleteContingentAsync(Guid id);
    }
}
