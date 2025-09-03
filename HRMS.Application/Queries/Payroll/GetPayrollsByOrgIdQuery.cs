using HRMS.Domain.Entities.Payroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Payroll
{
    public class GetPayrollsByOrgIdQuery : IRequest<List<PayrollModel>>
    {
        public Guid OrganizationId { get; set; }
        public GetPayrollsByOrgIdQuery(Guid organizationId)
        {
            OrganizationId = organizationId;
        }
    }
}
