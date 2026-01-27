using ProPlan.Entities.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Abstracts
{
    public interface ITaskDefinitionService
    {
        Task<List<TaskDefinitionDtoForRead>> GetAllTaskDefinitionsAsync();
        Task<TaskDefinitionDtoForRead?> GetTaskDefinitionByIdAsync(int id);
        Task<TaskDefinitionDtoForRead> CreateTaskDefinitionAsync(TaskDefinitionDtoForCreate dto);
        Task UpdateTaskDefinitionAsync(TaskDefinitionDtoForUpdate dto);
        Task DeleteTaskDefinitionAsync(int id);
    }

}
