using HRMS.Application.Commands.Designations;
using HRMS.Domain.Entities.Designations;
using HRMS.Infrastructure.Interfaces.Designations;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Designations
{
    public class CreateDesignationCommandHandler : IRequestHandler<CreateDesignationCommand , Guid>
    {
        private readonly IDesignationRepository _repository;
        public CreateDesignationCommandHandler(IDesignationRepository designationRepository) 
        {
            _repository = designationRepository;    
        }
        public async Task<Guid> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
        {
            var designation = new DesignationModel
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Code = request.Code,
                Description = request.Description,
                DepartmentId = request.DepartmentId,
                Level = request.Level,
                IsBillable = request.IsBillable,
                Status = request.Status,
                OrganizationId = request.OrganizationId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.CreateDesignationAsync(designation);
            return designation.Id;
        }

    }
}
