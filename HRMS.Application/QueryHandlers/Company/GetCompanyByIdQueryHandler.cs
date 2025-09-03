using HRMS.Application.Queries.Company;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Company
{
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, GetCompanyModel>
    {
        private readonly ICompanyRepository _repo;

        public GetCompanyByIdQueryHandler(ICompanyRepository repo)
        {
            _repo = repo ;
        }

        public async Task<GetCompanyModel> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _repo.GetByIdAsync(request.Id);
            if (company == null) return null;
            return new GetCompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                OrganizationId = company.OrganizationId,
                DivisionId = company.DivisionId,
                LegalName = company.LegalName,
                CountryCode = company.CountryCode,
                CurrencyCode = company.CurrencyCode,
                Email = company.Email,
                IsActive = company.IsActive
            };
        }
    }
}
