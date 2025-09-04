using HRMS.Application.Commands.Departments;
using HRMS.Application.Commands.Designations;
using HRMS.Domain.Entities.Departments;
using HRMS.Domain.Entities.Designations;
using HRMS.Infrastructure.Interfaces.Departments;
using HRMS.Infrastructure.Interfaces.Designations;
using HRMS.Infrastructure.Repositories.Designations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Designations
{
    public class UpdatedesignationCommandHandler : IRequestHandler<UpdateDesignationCommand>
    {
         private readonly IDesignationRepository _designationRepository;

        public UpdatedesignationCommandHandler(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository;
        }

        public async Task Handle(UpdateDesignationCommand request, CancellationToken cancellationToken)
        {
            var designation = new DesignationModel
            { 
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                Title = request.Title,
                Code = request.Code,
                Description = request.Description,
                DepartmentId = request.DepartmentId,
                Level = request.Level,
                IsBillable = request.IsBillable,
                Status = request.Status,
                UpdatedAt = DateTime.UtcNow
            };

            await _designationRepository.UpdateDesignationAsync(designation);
        }
    }
}
