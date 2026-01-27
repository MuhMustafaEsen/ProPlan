using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Abstract
{
    public interface ITaskSubItemRepository : IRepositoryBase<TaskSubItem>
    {
        Task<List<TaskSubItemDtoForRead>> GetTaskSubItemsByAssignmentId(int id);

        Task<bool> AreAllCompletedbyAssignmentIdAsync(int assignmentId);
        Task<bool> ArePreviousOrdersCompleted(int taskAssignmentId,int order);
    }
}
