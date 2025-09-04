using HRMS.Application.Commands.Location;
using HRMS.Infrastructure.Interfaces.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Location
{
    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, bool>
    {
        private readonly ILocationRepository _locationRepository;
        public UpdateLocationCommandHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = new Domain.Entities.Location.LocationModel
            {
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                DivisionId = request.DivisionId,
                CompanyId = request.CompanyId,
                LocationCode = request.LocationCode,
                LocationName = request.LocationName,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                CountryCode = request.CountryCode,
                Timezone = request.Timezone,
                IsPrimary = request.IsPrimary,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                GpsLatitude = request.GpsLatitude,
                GpsLongitude = request.GpsLongitude
            };
     
           
            await _locationRepository.UpdateLocationAsync(location);
            return true; // Update successful
        }
    }
}
