using ProPlan.Repositories.Abstract;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Contracts
{
    public class ServiceManager : IServiceManager   
    {
        public IUserService Users { get; }
        public ICompanyService Companies { get; }
        public ICompanyTaskService CompanyTasks { get; }
        public ITaskAssignmentService TaskAssignments { get; }
        public ITaskDefinitionService TaskDefinitions { get; }
        public ITaskSubItemService TaskSubItems { get; }
        public IDashboardService Dashboards { get; }

        public ServiceManager(IUserService userService,ICompanyService companyService, 
            ICompanyTaskService companyTaskService, ITaskAssignmentService taskAssignments, 
            ITaskDefinitionService taskDefinitions, IDashboardService dashboards,
            ITaskSubItemService taskSubItems)
        {
            Users = userService;
            Companies = companyService;
            CompanyTasks = companyTaskService;
            TaskAssignments = taskAssignments;
            TaskDefinitions = taskDefinitions;
            TaskSubItems = taskSubItems;
            Dashboards = dashboards;
        }
    }
}
