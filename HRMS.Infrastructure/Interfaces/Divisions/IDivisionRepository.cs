using HRMS.Domain.Entities.Divisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Divisions
{
    public interface IDivisionRepository
    {
        Task AddAsync(Division division);

        Task<Division> GetByIdAsync(Guid id);

        Task<List<Division>> GetAllByIdAsync(Guid id);

        Task UpdateAsync(Division division);

        Task<bool> DeleteAsync(Guid id);
    }
}
