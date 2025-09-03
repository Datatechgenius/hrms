using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.PayrollWages
{
    public class UpdatePayrollWagesCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid PayrollId { get; set; }
        public Guid EmployeeId { get; set; }
        public string WageType { get; set; }
        public decimal WageAmount { get; set; }
        public decimal? HoursWorked { get; set; }
        public decimal? RatePerHour { get; set; }
        public bool Taxable { get; set; }
        public string Remarks { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
