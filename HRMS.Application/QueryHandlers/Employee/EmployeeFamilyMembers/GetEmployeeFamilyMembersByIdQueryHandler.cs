using HRMS.Application.Queries.Employee;
using HRMS.Domain.Entities.Employee;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.Employee;
using HRMS.Infrastructure.Repositories.Employee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Employee
{
    public class GetEmployeesByFamilyMembersQueryHandler : IRequestHandler<GetEmployeeFamilyMembersByIdQuery, EmployeeFamilyMembers>
    {
        private readonly IEmployeeFamilyMembersRepository _repo;

        public GetEmployeesByFamilyMembersQueryHandler(IEmployeeFamilyMembersRepository repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeFamilyMembers> Handle(GetEmployeeFamilyMembersByIdQuery request, CancellationToken cancellationToken)
        {
            var models = await _repo.GetByIdAsync(request.Id);
            return models;
        }
    }
}