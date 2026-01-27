using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Abstract
{
    public interface IDashboardRepository
    {
        Task<List<CompanyProgressDto>> GetCompanyProgressAsync();
        Task<List<MonthlyCompletionDto>> GetMonthlyCompletionAsync();
        Task<List<UserPerformanceDto>> GetUserPerformanceAsync();
    }
}
