using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface ICompanyTaskService
    {
        Task<List<CompanyTaskDtoForRead>> GetAllCompanyTasksAsync();
        Task<CompanyTaskDtoForRead?> GetCompanyTaskByIdAsync(int id);
        Task<CompanyTaskDtoForRead> CreateCompanyTaskAsync(CompanyTaskDtoForCreate dto);
        Task UpdateCompanyTaskAsync(CompanyTaskDtoForUpdate dto);
        Task DeleteCompanyTaskAsync(int id);
    }

}
