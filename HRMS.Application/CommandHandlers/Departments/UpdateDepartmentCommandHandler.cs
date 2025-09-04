using HRMS.Application.Commands.Company;
using HRMS.Application.Commands.Departments;
using HRMS.Domain.Entities.Company;
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
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var company = new DepartmentModel
            {
               Id = request.Id,
               Name = request.Name,
               Code = request.Code,
               OrganizationId = request.OrganizationId,
               DivisionId = request.DivisionId,
               CompanyId = request.CompanyId,
               Email = request.Email,
               Phone = request.Phone,
               IsActive = request.IsActive,
               UpdatedAt = DateTime.UtcNow
            };

            await _departmentRepository.UpdateDepartmentAsync(company);
        }
    }
}
