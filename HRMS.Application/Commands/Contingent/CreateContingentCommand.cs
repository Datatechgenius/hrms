using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Contingent
{
    public class CreateContingentCommand : IRequest<Guid>
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool isBillable { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
