using HRMS.Application.Commands.Location;
using HRMS.Domain.Entities.Location;
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
    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, Guid>
    {
        private readonly ILocationRepository _repository;

        public CreateLocationCommandHandler(ILocationRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var entity = new LocationModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                DivisionId = request.DivisionId,
                CompanyId = request.CompanyId,
                LocationCode = request.LocationCode,
                LocationName = request.LocationName,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                //Country = request.Country,
                ZipCode = request.ZipCode,
                CountryCode = request.CountryCode,
                Timezone = request.Timezone,
                IsPrimary = request.IsPrimary,
                ContactNumber = request.ContactNumber,
                Email = request.Email,
                GpsLatitude = request.GpsLatitude,
                GpsLongitude = request.GpsLongitude,
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddLocationAsync(entity);
            return entity.Id;
        }
    }
}
