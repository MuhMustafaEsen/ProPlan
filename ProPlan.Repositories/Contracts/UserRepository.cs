using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext context): base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(
            string email, bool trackChanges)
        {
            return await FindByCondition(
                    u => u.Email == email,
                    trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}
