using HRMS.Domain.Entities.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Company
{
    public interface ICompanyRepository
    {
        Task CreateCompanyAsync(CompanyModel company);
        Task UpdateCompanyAsync(CompanyModel company);
        //Task<bool> DeleteCompanyAsync(Guid companyId);
        Task<GetCompanyModel> GetByIdAsync(Guid companyId);
        Task<bool> DeleteAsync(Guid id);
        //Task<IEnumerable<CompanyModel>> GetAllCompaniesAsync();
        Task<List<CompanyModel>> GetCompaniesByOrganizationIdAsync(Guid OrgId);
        Task<List<CompanyModel>> GetCompaniesByDivisionIdAsync(Guid divisionId);
    }
}
