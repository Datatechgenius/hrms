using HRMS.Application.Commands.User;
using HRMS.Domain.Entities.User;
using HRMS.Infrastructure.Interfaces.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UsersModel
            {
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                EmployeeId = request.EmployeeId,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                IsActive = request.IsActive,
                IsLocked = request.IsLocked,
                LastLoginAt = request.LastLoginAt,
                UpdatedAt = DateTime.UtcNow
            };
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
