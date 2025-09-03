using HRMS.Application.Queries.Departments;
using HRMS.Application.Queries.JobTitle;
using HRMS.Domain.Entities.Departments;
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
    public class GetJobTitleByCompanyIdQueryHandler : IRequestHandler<GetJobTitleByCompanyIdQuery, List<JobTitleModel>>
    {
        private readonly IJobTitleRepository _repository;
        public GetJobTitleByCompanyIdQueryHandler(IJobTitleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JobTitleModel>> Handle(GetJobTitleByCompanyIdQuery request,CancellationToken cancellationToken)
        {
            var jobTitles = await _repository.GetJobTitleByCompanyIdAsync(request.Id);

                        if (jobTitles == null || jobTitles.Count == 0)
                            return new List<JobTitleModel>();

                        return jobTitles
                            .Select(d => new JobTitleModel
                            {
                                Id = d.Id,
                                OrganizationId = d.OrganizationId,
                                DivisionId = d.DivisionId,
                                CompanyId = d.CompanyId,
                                Title = d.Title,
                                Description = d.Description,
                                Level = d.Level,
                                JobCode = d.JobCode,
                                PayGradeId = d.PayGradeId,
                                DepartmentId = d.DepartmentId,
                                IsActive = d.IsActive,
                                CreatedAt = d.CreatedAt,
                                UpdatedAt = d.UpdatedAt
                            })
                        .ToList(); 
            
        }
    }
}
