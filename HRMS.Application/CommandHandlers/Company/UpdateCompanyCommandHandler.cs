using HRMS.Application.Commands.Company;
using HRMS.Domain.Entities.Company;
using HRMS.Infrastructure.Interfaces.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Company
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new CompanyModel
            {
                Id = request.Id,
                Name = request.Name,
                Code = request.Code,
                OrganizationId = request.OrganizationId,
                DivisionId = request.DivisionId,
                LegalName = request.LegalName,
                TaxId = request.TaxId,
                DunsNumber = request.DunsNumber,
                IncorporationDate = request.IncorporationDate,
                CountryCode = request.CountryCode,
                CurrencyCode = request.CurrencyCode,
                Timezone = request.Timezone,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                Phone = request.Phone,
                Email = request.Email,
                Website = request.Website,
                IsActive = request.IsActive,
                UpdatedAt = DateTime.UtcNow
            };

            await _companyRepository.UpdateCompanyAsync(company);
        }
    }
}
