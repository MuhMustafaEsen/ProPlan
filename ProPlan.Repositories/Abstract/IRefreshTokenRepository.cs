using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Abstract
{
    public interface IRefreshTokenRepository: IRepositoryBase<RefreshToken>
    {
        Task<RefreshToken?> GetValidTokenAsync(string token);
    }

}
