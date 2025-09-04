using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.PayrollDeduction
{
    public class PayrollDeductionModel
    {
        public Guid Id { get; set; }
        public Guid PayrollId { get; set; }
        public string ComponentName { get; set; }
        public decimal Amount { get; set; }
    }
}
