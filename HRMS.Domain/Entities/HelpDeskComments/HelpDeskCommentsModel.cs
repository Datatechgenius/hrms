using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.HelpDeskComments
{
    public class HelpDeskCommentsModel
    {
        public Guid Id { get; set; }                    
        public Guid TicketId { get; set; }              
        public Guid EmployeeId { get; set; }            
        public string Comment { get; set; }             
        public DateTime CreatedAt { get; set; }         
    }
}
