using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.PayslipHistory
{
    public class PaySlipHistoryModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid PayrollId { get; set; }
        public int Version { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public DateTime GeneratedAt { get; set; }
        public Guid? GeneratedBy { get; set; }
        public string PayslipUrl { get; set; }
        public PaymentStatusEnum? DeliveryStatus { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public enum PaymentStatusEnum
    {
        unpaid,
        paid,
        partial
    }
}
