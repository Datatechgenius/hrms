using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.Divisions
{
    public class Division
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OrganizationId { get; set; }
        public string DivisionCode { get; set; }
        public string ShortName { get; set; }
        public bool? Active { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //public Guid LocationId { get; set; }
        public string Timezone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class DivisionResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public bool IsActive { get; set; }
        public string DivisionCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
