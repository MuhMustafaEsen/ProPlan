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
    public class RefreshTokenRepository: RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(RepositoryContext context)
            : base(context)
        {
        }

        public async Task<RefreshToken?> GetValidTokenAsync(string token)
        {
            return await FindByCondition(
                    x => x.Token == token &&
                         !x.IsRevoked &&
                         x.Expires > DateTime.UtcNow,
                    trackChanges: true)
                .Include(x => x.User)
                .SingleOrDefaultAsync();
        }
    }

}
