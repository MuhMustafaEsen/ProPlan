using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Contracts
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public TaskAssignmentService(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TaskAssignmentDtoForRead>> GetAllTaskAssignmentsAsync()
        {
            var assignments = await _repository.TaskAssignments
                .FindAll(trackChanges: false)
                .Include(a => a.CompanyTask)
                    .ThenInclude(ct => ct.Company)
                .Include(a => a.CompanyTask)
                    .ThenInclude(ct => ct.TaskDefinition)
                .Include(a => a.User)
                .ToListAsync();

            return _mapper.Map<List<TaskAssignmentDtoForRead>>(assignments);
        }

        public async Task<TaskAssignmentDtoForRead?> GetTaskAssignmentByIdAsync(int id)
        {
            var assignment = await _repository.TaskAssignments
                .FindByCondition(a => a.Id == id, false)
                .Include(a => a.CompanyTask)
                .ThenInclude(ct => ct.Company)
                .Include(a => a.CompanyTask)
                .ThenInclude(ct => ct.TaskDefinition)
                .Include(a => a.User)
                .SingleOrDefaultAsync();

            return assignment == null
                ? null
                : _mapper.Map<TaskAssignmentDtoForRead>(assignment);
        }

        public async Task<TaskAssignmentDtoForRead> CreateTaskAssignmentAsync(
            TaskAssignmentDtoForCreate dto)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                var entity = _mapper.Map<TaskAssignment>(dto);
                entity.IsCompleted = false;
                entity.CompletedAt = null;

                _repository.TaskAssignments.Create(entity);
                await _repository.SaveAsync();

                await _repository.CommitTransactionAsync();

                return _mapper.Map<TaskAssignmentDtoForRead>(entity);
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateTaskAssignmentAsync(TaskAssignmentDtoForUpdate dto)
        {
            var entity = await _repository.TaskAssignments
                .FindByCondition(a => a.Id == dto.Id, true)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new Exception("TaskAssignment not found");

            // completed logic
            if (!entity.IsCompleted && dto.IsCompleted)
            {
                entity.CompletedAt = DateTime.UtcNow;
            }

            if (entity.IsCompleted && !dto.IsCompleted)
            {
                entity.CompletedAt = null;
            }

            _mapper.Map(dto, entity);

            // tekrar kontrol sonradan eklenen 4 satır için
            if (entity.IsCompleted && entity.CompletedAt == null)
                entity.CompletedAt = DateTime.UtcNow;

            if (!entity.IsCompleted)
                entity.CompletedAt = null;

            await _repository.SaveAsync();
        }

        public async Task DeleteTaskAssignmentAsync(int id)
        {
            var entity = await _repository.TaskAssignments
                .FindByCondition(a => a.Id == id, true)
                .SingleOrDefaultAsync();

            if (entity != null)
            {
                _repository.TaskAssignments.Delete(entity);
                await _repository.SaveAsync();
            }
        }
        public async Task<List<TaskAssignmentDtoForRead>> GetMyTaskAssignmentsAsync(int userId)
        {
            var assignments = await _repository.TaskAssignments
                .FindByCondition(a => a.UserId == userId, false)
                .Include(a => a.CompanyTask)
                    .ThenInclude(ct => ct.Company)
                .Include(a => a.User)
                .OrderBy(a => a.TaskDate)
                .ToListAsync();

            return _mapper.Map<List<TaskAssignmentDtoForRead>>(assignments);
        }
        public async Task CompleteTaskAssignmentAsync(int id, int userId)
        {
            var assignment = await _repository.TaskAssignments
                .FindByCondition(a => a.Id == id, true)
                .SingleOrDefaultAsync();

            if (assignment == null)
                throw new Exception("TaskAssignment not found");

            // başkasının görevini kapatmasın
            if (assignment.UserId != userId)
                throw new UnauthorizedAccessException();

            if (assignment.IsCompleted)
                return;

            assignment.IsCompleted = true;
            assignment.CompletedAt = DateTime.UtcNow;

            await _repository.SaveAsync();
        }

        public async Task<List<MonthlyTaskAssignmentDto>> GetMonthlyTasksAsync(MonthlyTaskFilterDto filter)
        {
            var startDate = new DateTime(filter.Year, filter.Month, 1);
            var endDate = startDate.AddMonths(1);

            var query = _repository.TaskAssignments
                .FindByCondition(
                    t => t.TaskDate >= startDate && t.TaskDate < endDate,
                    trackChanges: false)
                .Include(t => t.CompanyTask)
                    .ThenInclude(ct => ct.Company)
                .Include(t => t.CompanyTask)
                    .ThenInclude(ct => ct.TaskDefinition)
                .Include(t => t.User)
                .AsQueryable();

            if (filter.CompanyId.HasValue)
            {
                query = query.Where(t =>
                    t.CompanyTask.CompanyId == filter.CompanyId.Value);
            }

            if (filter.UserId.HasValue)
            {
                query = query.Where(t => t.UserId == filter.UserId.Value);
            }

            var entities = await query.ToListAsync();

            return _mapper.Map<List<MonthlyTaskAssignmentDto>>(entities);
        }
    }
}
