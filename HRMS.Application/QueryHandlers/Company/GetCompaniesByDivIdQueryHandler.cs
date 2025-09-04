using HRMS.Application.Queries.Company;
using HRMS.Application.Queries.Divisions;
using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Divisions;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Divisions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Company
{
    public class GetCompaniesByDivIdQueryHandler : IRequestHandler<GetCompaniesByDivIdQuery, List<GetCompanyModel>>
    {
        private readonly ICompanyRepository _repository;

        public GetCompaniesByDivIdQueryHandler(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetCompanyModel>> Handle(GetCompaniesByDivIdQuery request,CancellationToken cancellationToken)
        {
            var companies = await _repository.GetCompaniesByDivisionIdAsync(request.Id);

            if (companies == null || companies.Count == 0)
                return new List<GetCompanyModel>();

            return companies
                .Select(d => new GetCompanyModel
                {
                    Id              = d.Id,
                    Name            = d.Name,
                    OrganizationId  = d.OrganizationId,
                    DivisionId      = d.DivisionId,
                    CountryCode     = d.CountryCode,
                    CurrencyCode    = d.CurrencyCode,
                    Email           = d.Email,
                    LegalName       = d.LegalName,
                    IsActive        = d.IsActive,
                    CreatedAt       = d.CreatedAt,
                })
                .ToList();
        }
    }
}
