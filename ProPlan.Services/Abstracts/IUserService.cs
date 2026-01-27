using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface IUserService
    {
        Task<List<UserDtoForRead>> GetAllUsersAsync();
        Task<UserDtoForRead?> GetUserByIdAsync(int id);
        Task<UserDtoForRead> CreateUserAsync(UserDtoForCreate userDtoForCreate);
        Task UpdateUserAsync(UserDtoForUpdate userDtoForUpdate);
        Task DeleteUserAsync(int id);
    }
}
