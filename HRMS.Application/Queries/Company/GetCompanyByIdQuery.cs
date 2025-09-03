using HRMS.Domain.Entities.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Company
{
    public class GetCompanyByIdQuery : IRequest<GetCompanyModel>
    {
        public Guid Id { get; }
        public GetCompanyByIdQuery(Guid id)
        { 
            Id = id;
        }
    }
}
