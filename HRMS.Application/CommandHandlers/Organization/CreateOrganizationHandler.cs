using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using HRMS.Application.Commands;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;

namespace HRMS.Application.CommandHandlers
{
    public class CreateOrganizationHandler : IRequestHandler<CreateOrganizationCommand, Guid>
    {
        private readonly IOrganizationRepository _repo;

        public CreateOrganizationHandler(IOrganizationRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateOrganizationCommand cmd, CancellationToken ct)
        {
            var id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id;

            var org = new OrganizationModel
            {
                Id                 = id,
                Name               = cmd.Name,
                Description        = cmd.Description,
                //CompanyId          = cmd.CompanyId,
                TaxId              = cmd.TaxId,
                Phone              = cmd.Phone,
                Email              = cmd.Email,
                Website            = cmd.Website,
                Industry           = cmd.Industry,
                OrgType            = cmd.OrgType,
                Size               = cmd.Size,
                IncorporationDate  = cmd.IncorporationDate,
                AddressLine1       = cmd.AddressLine1,
                AddressLine2       = cmd.AddressLine2,
                City               = cmd.City,
                State              = cmd.State,
                ZipCode            = cmd.ZipCode,
                CountryCode        = cmd.CountryCode,
                Timezone           = cmd.Timezone,
                CurrencyCode       = cmd.CurrencyCode,
                LogoUrl            = cmd.LogoUrl,
                Status             = cmd.Status
            };

            return await _repo.OrganizationInsertAsync(org);
        }
    }
}