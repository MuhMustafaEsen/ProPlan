using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface IServiceManager
    {
        IUserService Users { get; }
        ICompanyService Companies { get; }
        ICompanyTaskService CompanyTasks { get; }

        ITaskDefinitionService TaskDefinitions { get; }
        ITaskAssignmentService TaskAssignments { get; }
        ITaskSubItemService TaskSubItems { get; }

        IDashboardService Dashboards { get; }

    }
}
