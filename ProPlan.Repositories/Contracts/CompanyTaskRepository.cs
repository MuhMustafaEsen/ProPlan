using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class CompanyTaskRepository
     : RepositoryBase<CompanyTask>, ICompanyTaskRepository
    {
        public CompanyTaskRepository(RepositoryContext context)
            : base(context)
        {
        }
    }

}
