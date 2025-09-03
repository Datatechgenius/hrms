using HRMS.Domain.Entities.Contingent;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Contingent
{
    public class GetContingentByIdQuery : IRequest<ContingentDto>
    {
        public Guid Id { get; set; }

        public GetContingentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
