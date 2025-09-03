using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.Payroll
{
    public class PayrollModel
    {
        public Guid Id { get; set; }                          
        public Guid OrganizationId { get; set; }              
        public Guid CompanyId { get; set; }                   
        public DateTime PayrollMonth { get; set; }            
        public string PayrollName { get; set; }               
        public PayrollRunStatusEnum? Status { get; set; }      
        public Guid? ProcessedBy { get; set; }                
        public DateTime? ProcessedDate { get; set; }          
        public int TotalEmployees { get; set; }               
        public decimal TotalWages { get; set; }               
        public decimal TotalDeductions { get; set; }          
        public decimal TotalNetPay { get; set; }              
        public DateTime? PayDate { get; set; }                
        public string Remarks { get; set; }                   
        public bool IsReversal { get; set; }                  
        public Guid? OriginalPayrollId { get; set; }          
        public DateTime? CreatedAt { get; set; }              
        public DateTime? UpdatedAt { get; set; }              
    }

    public enum PayrollRunStatusEnum
    {
        pending,
        finalized
    }
}
