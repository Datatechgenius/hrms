using HRMS.Application.Commands.Departments;
using HRMS.Domain.Entities.Departments;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Departments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Departments
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Guid>
    {
        private readonly IDepartmentRepository _repository;

        public CreateDepartmentCommandHandler(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
           var department = new DepartmentModel
           {
               Id = Guid.NewGuid(),
               Name = request.Name,
               Code = request.Code,
               OrganizationId = request.OrganizationId,
               DivisionId = request.DivisionId,
               CompanyId = request.CompanyId,
               Email = request.Email,
               Phone = request.Phone,
               IsActive = request.IsActive,
               CreatedAt = DateTime.UtcNow,
               UpdatedAt = DateTime.UtcNow
           };
            await _repository.CreateDepartmentAsync(department);
            return department.Id;
        }
    }
}
