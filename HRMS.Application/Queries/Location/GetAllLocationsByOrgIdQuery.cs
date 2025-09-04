using HRMS.Domain.Entities.Location;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Location
{
    public class GetAllLocationsByOrgIdQuery : IRequest<List<LocationModel>>
    {
        public Guid OrganizationId { get; set; }
        public GetAllLocationsByOrgIdQuery(Guid organizationId)
        {
            OrganizationId = organizationId;
        }
    }
}
