using HRMS.Application.Commands;
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
   public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Guid>
    {
        private readonly ICompanyRepository _repository;

        public CreateCompanyCommandHandler(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = new CompanyModel
            {
                Id = Guid.NewGuid(),
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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repository.CreateCompanyAsync(company);
            return company.Id;
        }
    }
}
