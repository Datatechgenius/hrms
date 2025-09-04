using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Organization
{
     public class DeleteOrganizationCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

}
