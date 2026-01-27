using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Abstract
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        ICompanyRepository Companies { get; }

        ICompanyTaskRepository CompanyTasks { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        ITaskDefinitionRepository TaskDefinitions { get; }
        ITaskAssignmentRepository TaskAssignments { get; }
        ITaskSubItemRepository TaskSubItems { get; }
        IDashboardRepository Dashboards { get; }
        Task SaveAsync();       
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
