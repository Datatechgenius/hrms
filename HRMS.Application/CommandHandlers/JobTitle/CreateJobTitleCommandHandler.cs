using HRMS.Application.Commands.JobTitle;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Infrastructure.Interfaces.JobTitle;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.JobTitle
{
    public class CreateJobTitleCommandHandler : IRequestHandler<CreateJobTitleCommand, Guid>
    {
        private readonly IJobTitleRepository _repo;

        public CreateJobTitleCommandHandler(IJobTitleRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateJobTitleCommand cmd, CancellationToken ct)
        {
            var entity = new JobTitleModel
            {
                Id = Guid.NewGuid(),
                OrganizationId = cmd.OrganizationId,
                DivisionId = cmd.DivisionId,
                CompanyId = cmd.CompanyId,
                Title = cmd.Title,
                Description = cmd.Description,
                Level = cmd.Level,
                JobCode = cmd.JobCode,
                PayGradeId = cmd.PayGradeId,
                DepartmentId = cmd.DepartmentId,
                IsActive = cmd.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateJobTitleAsync(entity);
            return entity.Id;
        }
    }

}
