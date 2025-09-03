using HRMS.Domain.Entities.Divisions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Divisions
{
    public class GetAllDivisionByIdQuery : IRequest<List<DivisionResponseDto>>
    {
        public Guid Id { get; set; }

        public GetAllDivisionByIdQuery(Guid id)
        {
            Id = id;
        }
    }

}
