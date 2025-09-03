using HRMS.Application.Commands.Organization;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Organization
{
    public class UpdateOrganizationHandler 
        : IRequestHandler<UpdateOrganizationCommand, bool>
    {
        private readonly IOrganizationRepository _repo;

        public UpdateOrganizationHandler(IOrganizationRepository repo)
            => _repo = repo;

        public async Task<bool> Handle(UpdateOrganizationCommand cmd, 
                                       CancellationToken ct)
        {
            var org = new OrganizationModel
            {
                Id                 = cmd.Id,
                Name               = cmd.Name,
                Description        = cmd.Description,
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

            return await _repo.UpdateAsync(org);
        }
    }
}
