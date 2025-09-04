using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Employee.EmployeeFamilyMember
{
    public class GetAllEmployeeFamilyMembersByIdQueryHandler : IRequestHandler<GetAllEmployeeFamilyMembersByIdQuery, List<EmployeeFamilyMembers>>
    {
        private readonly IEmployeeFamilyMembersRepository _repo;

        public GetAllEmployeeFamilyMembersByIdQueryHandler(IEmployeeFamilyMembersRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeFamilyMembers>> Handle(GetAllEmployeeFamilyMembersByIdQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetAllByEmployeeIdAsync(request.Id);
            return models
                .Select(model => new EmployeeFamilyMembers
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DisplayName = model.DisplayName,
                    RelationType = model.RelationType,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Profession = model.Profession,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    State = model.State,
                    CountryCode = model.CountryCode,
                    ZipCode = model.ZipCode,
                    IsDependent = model.IsDependent,
                    IsEmergency = model.IsEmergency,
                    IsNominee = model.IsNominee,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt
                })
                .ToList();
        }
    }
}