using HRMS.Application.Commands;
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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentException("Invalid user data.");
            }
            var user = new Domain.Entities.User.UsersModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                EmployeeId = request.EmployeeId,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                IsActive = request.IsActive,
                IsLocked = request.IsLocked,
                LastLoginAt = request.LastLoginAt,
                CreatedAt = DateTime.UtcNow
            };
            // Save the user to the repository
            await _userRepository.AddUserAsync(user);
            // Return the created user's ID
            return user.Id;
        }
    }
}
