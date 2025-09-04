using HRMS.Domain.Entities.PayrollDeduction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayrollDeduction
{
    public class GetPayrollDeductionByIdQuery : IRequest<PayrollDeductionModel>
    {
        public Guid Id { get; set; }
        public GetPayrollDeductionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
