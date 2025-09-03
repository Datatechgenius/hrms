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
    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationModel>
    {
        private readonly ILocationRepository _repository;
        public GetLocationByIdQueryHandler(ILocationRepository repository)
        {
            _repository = repository;
        }
        public async Task<LocationModel> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _repository.GetLocationByIdAsync(request.Id);

            if (location == null) {
                throw new KeyNotFoundException($"Location with ID {request.Id} not found.");
            }
            return new LocationModel{
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
            };
        }
    }
}
