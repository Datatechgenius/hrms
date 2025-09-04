using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.User
{
    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
