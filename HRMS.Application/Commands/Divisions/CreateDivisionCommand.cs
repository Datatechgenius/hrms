using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Divisions
{
    public class CreateDivisionCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public Guid OrganizationId { get; set; }
        public bool IsActive { get; set; }
        public string DivisionCode { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Timezone { get; set; }
    }
}
