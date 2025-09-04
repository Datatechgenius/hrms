using HRMS.Domain.Entities.PayslipHistory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.PayslipHistory
{
    public class GetPayslipHistoryByIdQuery : IRequest<PaySlipHistoryModel>
    {
        public Guid Id { get; set; }
        public GetPayslipHistoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
