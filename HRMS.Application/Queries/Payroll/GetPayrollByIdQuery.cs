using HRMS.Domain.Entities.Payroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Payroll
{
    public class GetPayrollByIdQuery : IRequest<PayrollModel>
    {
        public Guid Id { get; set; }
        public GetPayrollByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
