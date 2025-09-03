using HRMS.Domain.Entities.JobTitle;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.JobTitle
{
    public class GetJobTitleByCompanyIdQuery : IRequest<List<JobTitleModel>>
    {
        public Guid Id { get; set; }
        public GetJobTitleByCompanyIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
