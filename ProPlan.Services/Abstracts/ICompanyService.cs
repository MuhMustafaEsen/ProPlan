using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface ICompanyService
    {
        Task<List<CompanyDtoForRead>> GetAllCompaniesAsync();
        Task<CompanyDtoForRead?> GetCompanyByIdAsync(int id);
        Task<CompanyDtoForRead> CreateCompanyAsync(CompanyDtoForCreate companyDto);
        Task UpdateCompanyAsync(CompanyDtoForUpdate companyDto);
        Task DeleteCompanyAsync(int id);
    }

}
