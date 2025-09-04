using HRMS.Domain.Entities.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Location
{
    public class GetLocationByIdQuery : IRequest<LocationModel>
    {
        public Guid Id { get; set; }
        public GetLocationByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
