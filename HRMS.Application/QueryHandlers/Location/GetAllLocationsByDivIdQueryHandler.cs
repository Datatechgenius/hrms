using HRMS.Application.Queries.Location;
using HRMS.Domain.Entities.Location;
using HRMS.Infrastructure.Interfaces.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Location
{
    public class GetAllLocationsByDivIdQueryHandler : IRequestHandler<GetAllLocationsByDivIdQuery, List<LocationModel>>
    {
        private readonly ILocationRepository _locationRepository;
        public GetAllLocationsByDivIdQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public async Task<List<LocationModel>> Handle(GetAllLocationsByDivIdQuery request, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.GetAllLocationsByDivIdAsync(request.DivisionId);

            if (locations == null)
            {
                return new List<LocationModel>();
            }
            return locations.Select(location => new LocationModel
            {
                Id = location.Id,
                OrganizationId = location.OrganizationId,
                DivisionId = location.DivisionId,
                CompanyId = location.CompanyId,
                LocationCode = location.LocationCode,
                LocationName = location.LocationName,
                AddressLine1 = location.AddressLine1,
                AddressLine2 = location.AddressLine2,
                City = location.City,
                State = location.State,
                ZipCode = location.ZipCode,
                CountryCode = location.CountryCode,
                Timezone = location.Timezone,
                IsPrimary = location.IsPrimary,
                ContactNumber = location.ContactNumber,
                Email = location.Email,
                GpsLatitude = location.GpsLatitude,
                GpsLongitude = location.GpsLongitude,
                CreatedAt = location.CreatedAt
            }).ToList();
        }
    }
}
