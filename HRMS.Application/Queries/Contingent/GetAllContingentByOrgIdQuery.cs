using HRMS.Domain.Entities.Contingent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Contingent
{
    public class GetAllContingentByOrgIdQuery : IRequest<List<ContingentDto>>
    {
        public Guid Id { get; set; }

        public GetAllContingentByOrgIdQuery(Guid id)
        {
            Id = id;
        }
    }

}
