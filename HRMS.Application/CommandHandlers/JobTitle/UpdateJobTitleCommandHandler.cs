using HRMS.Application.Commands.JobTitle;
using HRMS.Domain.Entities.Divisions;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Infrastructure.Interfaces.JobTitle;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.JobTitle
{
    public class UpdateJobTitleCommandHandler : IRequestHandler<UpdateJobTitleCommand>
    {
        private readonly IJobTitleRepository _repo;

        public UpdateJobTitleCommandHandler(IJobTitleRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(UpdateJobTitleCommand request, CancellationToken ct)
        {
            var jobTitle = new JobTitleModel
            {
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                DivisionId = request.DivisionId,
                CompanyId = request.CompanyId,
                Title = request.Title,
                Description = request.Description,
                Level = request.Level,
                JobCode = request.JobCode,
                PayGradeId = request.PayGradeId,
                DepartmentId = request.DepartmentId,
                IsActive = request.IsActive,
                UpdatedAt = DateTime.UtcNow
            };
            await _repo.UpdateJobTitleAsync(jobTitle);
        }
    }

}
