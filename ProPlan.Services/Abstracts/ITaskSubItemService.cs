using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface ITaskSubItemService
    {
        Task<List<TaskSubItemDtoForRead>> GetAllTaskSubItemAsync();
        Task<TaskSubItemDtoForRead?> GetTaskSubItemByIdAsync(int id);
        Task<List<TaskSubItemDtoForRead>> GetTaskSubItemsByAssignmentId(int id);
        Task<TaskSubItemDtoForCreate> CreateTaskSubItemAsync(TaskSubItemDtoForCreate dto);
        Task UpdateTaskSubItemAsync(TaskSubItemDtoForUpdate dto);
        Task UpdateTaskSubItemToggleAsync(TaskSubItemDtoForUpdate dto);
        Task DeleteTaskSubItemAsync(int id);
    }
}
