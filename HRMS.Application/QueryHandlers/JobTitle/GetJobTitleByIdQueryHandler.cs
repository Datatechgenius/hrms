using HRMS.Application.Queries.JobTitle;
using HRMS.Domain.Entities.JobTitle;
using HRMS.Infrastructure.Interfaces.JobTitle;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.JobTitle
{
    public class GetJobTitleByIdQueryHandler : IRequestHandler<GetJobTitleByIdQuery, JobTitleModel>
    {
        private readonly IJobTitleRepository _repo;

        public GetJobTitleByIdQueryHandler(IJobTitleRepository repo)
        {
            _repo = repo;
        }

        public async Task<JobTitleModel> Handle(GetJobTitleByIdQuery qry, CancellationToken ct)
        {
            var jobTitle = await _repo.GetByIdAsync(qry.Id);
            if (jobTitle == null) return null;

            return new JobTitleModel 
            {
                Id = jobTitle.Id,
                OrganizationId = jobTitle.OrganizationId,
                DivisionId = jobTitle.DivisionId,
                CompanyId = jobTitle.CompanyId,
                Title = jobTitle.Title,
                Description = jobTitle.Description,
                Level = jobTitle.Level,
                JobCode = jobTitle.JobCode,
                PayGradeId = jobTitle.PayGradeId,
                DepartmentId = jobTitle.DepartmentId,
                IsActive = jobTitle.IsActive,
                CreatedAt = jobTitle.CreatedAt,
                UpdatedAt = jobTitle.UpdatedAt
            };
        }
    }

}
