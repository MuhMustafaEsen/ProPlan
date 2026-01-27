using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class TaskDefinitionRepository
     : RepositoryBase<TaskDefinition>, ITaskDefinitionRepository
    {
        public TaskDefinitionRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

}
