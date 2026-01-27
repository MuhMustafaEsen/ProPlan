using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly RepositoryContext _context;

        public DashboardRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyProgressDto>> GetCompanyProgressAsync()
        {
            return await _context.Companies
                .Select(c => new CompanyProgressDto
                {
                    CompanyId = c.Id,
                    CompanyName = c.CompanyName,
                    CompletionRate = c.CompanyTasks.Count == 0
                        ? 0
                        : Math.Round(c.CompanyTasks.Count(t => t.TaskAssignments.Any(a => a.IsCompleted)) * 100.0
                          / c.CompanyTasks.Count,2)
                })
                .ToListAsync();
        }

        public async Task<List<MonthlyCompletionDto>> GetMonthlyCompletionAsync()
        {
            var data = await _context.TaskAssignments
                .Where(t => t.CompletedAt.HasValue)
                .GroupBy(t => new
                {
                    Year = t.CompletedAt.Value.Year,
                    Month = t.CompletedAt.Value.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalCount = g.Count(),
                    CompletedCount = g.Count(t => t.IsCompleted)
                })
                .ToListAsync();

            return data
                .Select(x => new MonthlyCompletionDto
                {
                    Month = $"{x.Year}-{x.Month:D2}",
                    Rate = x.TotalCount == 0
                        ? 0
                        : x.CompletedCount * 100.0 / x.TotalCount
                })
                .OrderBy(x => x.Month)
                .ToList();
        }


        public async Task<List<UserPerformanceDto>> GetUserPerformanceAsync()
        {
            return await _context.Users
                .Select(u => new UserPerformanceDto
                {
                    UserId = u.Id,
                    UserName = u.FirstName + " " + u.LastName,
                    Total = u.TaskAssignments.Count,
                    Completed = u.TaskAssignments.Count(t => t.IsCompleted),
                    Rate = u.TaskAssignments.Count == 0 ? 0 : u.TaskAssignments.Count(t => t.IsCompleted) * 100.0 / u.TaskAssignments.Count
                })
                .ToListAsync();
        }
    }


}
