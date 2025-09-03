using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.Employee
{
    public class EmployeeBankAccounts
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int? AccountType { get; set; }   
        public bool? IsPrimary { get; set; }   
        public string BankName { get; set; }   
        public string AccountNumber { get; set; }   
        public string AccountHolderName { get; set; }   
        public string BranchName { get; set; }   
        public string IfscCode { get; set; }   
        public string SwiftCode { get; set; }   
        public string RoutingNumber { get; set; }   
        public string MicrCode { get; set; }   
        public string CountryCode { get; set; }   
        public string CurrencyCode { get; set; }   
        public DateTime? EffectiveFrom { get; set; }   
        public DateTime? EffectiveTo { get; set; }   
        public bool? IsVerified { get; set; }   
        public string Notes { get; set; }   
        public DateTime? CreatedAt { get; set; }   
        public DateTime? UpdatedAt { get; set; }   
    }
}
