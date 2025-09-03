using HRMS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Organization
{
     public class GetOrganizationByIdQuery : IRequest<OrganizationModel>
    {
        public Guid Id { get; set; }
    }

}
