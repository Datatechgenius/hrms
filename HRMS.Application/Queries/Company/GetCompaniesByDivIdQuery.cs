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
    public class GetCompaniesByDivIdQuery : IRequest<List<GetCompanyModel>>
    {
        public Guid Id { get; set; }

        public GetCompaniesByDivIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
