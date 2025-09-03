using HRMS.Domain.Entities.Company;
using HRMS.Domain.Entities.Divisions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Company
{
    public class GetCompaniesByOrgIdQuery : IRequest<List<GetCompanyModel>>
    {
        public Guid Id { get; set; }

        public GetCompaniesByOrgIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
