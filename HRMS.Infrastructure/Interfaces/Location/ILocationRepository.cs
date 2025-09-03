using HRMS.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Location
{
    public interface ILocationRepository
    {
        Task AddLocationAsync(LocationModel location);
        Task<LocationModel> GetLocationByIdAsync(Guid Id);
        Task<List<LocationModel>> GetAllLocationsByOrgIdAsync(Guid Id);
        Task<List<LocationModel>> GetAllLocationsByDivIdAsync(Guid Id);
        Task UpdateLocationAsync(LocationModel location);
        Task<bool> DeleteLocationAsync(Guid Id);
    }
}
