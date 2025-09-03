using HRMS.Application.Commands.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Employee
{
    public class CreateEmployeeFamilyMembersCommandHandler : IRequestHandler<CreateEmployeeFamilyMemberCommand, Guid>
    {
        private readonly IEmployeeFamilyMembersRepository _repo;

        public CreateEmployeeFamilyMembersCommandHandler(IEmployeeFamilyMembersRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateEmployeeFamilyMemberCommand request, CancellationToken cancellationToken)
        {
            var entity = new EmployeeFamilyMembers
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DisplayName = request.DisplayNmae,
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
                CreatedAt = request.CreatedAt

            };

            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}