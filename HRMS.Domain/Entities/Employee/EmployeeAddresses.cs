using System;

namespace HRMS.Domain.Entities.Employee
{
    public class EmployeeAddresses
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }

     public class EmployeeAddressesDto
     {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public int AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
     }
}
