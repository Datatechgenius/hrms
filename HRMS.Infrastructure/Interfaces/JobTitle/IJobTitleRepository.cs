using HRMS.Domain.Entities.JobTitle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.JobTitle
{
    public interface IJobTitleRepository
    {
        Task<JobTitleModel> GetByIdAsync(Guid id);
        Task<List<JobTitleModel>> GetJobTitleByCompanyIdAsync(Guid id);
        Task CreateJobTitleAsync(JobTitleModel model);
        Task UpdateJobTitleAsync(JobTitleModel model);
        Task<bool> DeleteAsync(Guid id);
    }
}
