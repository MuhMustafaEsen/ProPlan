using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class TaskSubItemRepository :RepositoryBase<TaskSubItem>, ITaskSubItemRepository
    {
        public TaskSubItemRepository(RepositoryContext context)
            : base(context)
        {
            _context.TaskSubItems
            .Include(x => x.TaskAssignment)
            .ThenInclude(x => x.CompanyTask);
        }
        //tüm alt maddeler tamamlandı mı kontrolü
        public async Task<bool> AreAllCompletedbyAssignmentIdAsync(int assignmentId)
        {
            return !await _context.TaskSubItems
                .AnyAsync(tsi => tsi.TaskAssignmentId == assignmentId && !tsi.IsCompleted);       
        }

        public async Task<bool> ArePreviousOrdersCompleted(int taskAssignmentId,int order)
        {
            return !await _context.TaskSubItems
                .AnyAsync(x =>
                    x.TaskAssignmentId == taskAssignmentId &&
                    x.Order < order &&
                    !x.IsCompleted);
        }


        public async Task<List<TaskSubItemDtoForRead>> GetTaskSubItemsByAssignmentId(int id)
        {
            return await _context.TaskSubItems
                .Where(tsi => tsi.TaskAssignmentId == id)
                .Select(tsi => new TaskSubItemDtoForRead
                {
                    Id = tsi.Id,
                    TaskAssignmentId = tsi.TaskAssignmentId,
                    TaskDate = tsi.TaskAssignment.TaskDate,
                    IsCompleted = tsi.IsCompleted,
                    CompletedAt = tsi.CompletedAt,
                    Order = tsi.Order,
                    UserId = tsi.TaskAssignment.UserId,
                    CompanyTaskId = tsi.TaskAssignment.CompanyTaskId,
                    Title = tsi.Title,
                    Description = tsi.Description,
                })
                .ToListAsync();
        }
    }
}
