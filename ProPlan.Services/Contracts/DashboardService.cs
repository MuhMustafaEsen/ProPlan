using ProPlan.Entities.DataTransferObject;
using ProPlan.Repositories.Abstract;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Contracts
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepositoryManager _repository;

        public DashboardService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<List<CompanyProgressDto>> GetCompanyProgressAsync()
        {
            return await _repository.Dashboards.GetCompanyProgressAsync();
        }

        public async Task<List<MonthlyCompletionDto>> GetMonthlyCompletionAsync()
        {
            return await _repository.Dashboards.GetMonthlyCompletionAsync();
        }

        public async Task<List<UserPerformanceDto>> GetUserPerformanceAsync()
        {
            return await _repository.Dashboards.GetUserPerformanceAsync();
        }
    }
 }
