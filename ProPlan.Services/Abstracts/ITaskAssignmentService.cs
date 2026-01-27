using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface ITaskAssignmentService
    {
        Task<List<MonthlyTaskAssignmentDto>> GetMonthlyTasksAsync(
        MonthlyTaskFilterDto filter);
        Task<List<TaskAssignmentDtoForRead>> GetAllTaskAssignmentsAsync();
        Task<TaskAssignmentDtoForRead?> GetTaskAssignmentByIdAsync(int id);
        Task<List<TaskAssignmentDtoForRead>> GetMyTaskAssignmentsAsync(int userId);
        Task CompleteTaskAssignmentAsync(int id, int userId);
        Task<TaskAssignmentDtoForRead> CreateTaskAssignmentAsync(TaskAssignmentDtoForCreate dto);
        Task UpdateTaskAssignmentAsync(TaskAssignmentDtoForUpdate dto);
        Task DeleteTaskAssignmentAsync(int id);
    }

}
