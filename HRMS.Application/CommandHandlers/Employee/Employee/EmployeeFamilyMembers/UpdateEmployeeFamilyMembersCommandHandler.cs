using HRMS.Application.Commands.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class UpdateEmployeeFamilyMembersCommandHandler : IRequestHandler<UpdateEmployeeFamilyMembersCommand>
    {
        private readonly IEmployeeFamilyMembersRepository _repo;

        public UpdateEmployeeFamilyMembersCommandHandler(IEmployeeFamilyMembersRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(UpdateEmployeeFamilyMembersCommand request, CancellationToken cancellationToken)
        {
            var existing = new EmployeeFamilyMembers
            {
                Id = request.Id,
                EmployeeId = request.EmployeeId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DisplayName = request.DisplayName,
                RelationType = request.RelationType,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                Mobile = request.Mobile,
                Profession = request.Profession,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                CountryCode = request.CountryCode,
                ZipCode = request.ZipCode,
                IsDependent = request.IsDependent,
                IsEmergency = request.IsEmergency,
                IsNominee = request.IsNominee,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.UpdateAsync(existing);

        }
    }
}