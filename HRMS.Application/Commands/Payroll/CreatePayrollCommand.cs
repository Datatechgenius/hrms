using HRMS.Domain.Entities.Payroll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Payroll
{
    public class CreatePayrollCommand : IRequest<Guid>
    {
        public Guid OrganizationId { get; set; }             
        public Guid CompanyId { get; set; }                   
        public DateTime PayrollMonth { get; set; }            
        public string PayrollName { get; set; }               
        public PayrollRunStatusEnum Status { get; set; }      
        public Guid? ProcessedBy { get; set; }                
        public DateTime? ProcessedDate { get; set; }          
        public int TotalEmployees { get; set; }               
        public decimal TotalWages { get; set; }               
        public decimal TotalDeductions { get; set; }          
        public decimal TotalNetPay { get; set; }              
        public DateTime? PayDate { get; set; }                
        public string Remarks { get; set; }                   
        public bool IsReversal { get; set; }                   
    }
}
