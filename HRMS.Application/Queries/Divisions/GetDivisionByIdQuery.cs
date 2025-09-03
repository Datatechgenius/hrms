using HRMS.Domain.Entities.Divisions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Divisions
{
    public class GetDivisionByIdQuery : IRequest<DivisionResponseDto>
    {
        public Guid Id { get; set; }

        public GetDivisionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
