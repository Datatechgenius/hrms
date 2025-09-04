using HRMS.Domain.Entities.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Location
{
    public class GetAllLocationsByDivIdQuery : IRequest<List<LocationModel>>
    {
        public Guid DivisionId { get; set; }
        public GetAllLocationsByDivIdQuery(Guid divisionId)
        {
            DivisionId = divisionId;
        }
    }
}
