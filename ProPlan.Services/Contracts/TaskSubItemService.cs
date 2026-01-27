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
    public class TaskSubItemService : ITaskSubItemService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public TaskSubItemService(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TaskSubItemDtoForRead>> GetAllTaskSubItemAsync()
        {
            var taskSubItems = await _repository.TaskSubItems
                .FindAll(trackChanges: false)
                .Include(tsi => tsi.TaskAssignment) // TaskAssignment ilişkisini dahil ediyoruz
                .ToListAsync();

            return _mapper.Map<List<TaskSubItemDtoForRead>>(taskSubItems);
        }

        public async Task<TaskSubItemDtoForRead?> GetTaskSubItemByIdAsync(int id)
        {
            var taskSubItem = await _repository.TaskSubItems
                .FindByCondition(tsi => tsi.Id == id, trackChanges: false)
                .Include(tsi => tsi.TaskAssignment) // TaskAssignment ilişkisini dahil ediyoruz
                .SingleOrDefaultAsync();

            return taskSubItem == null
                ? null
                : _mapper.Map<TaskSubItemDtoForRead>(taskSubItem);
        }

        public async Task<TaskSubItemDtoForCreate> CreateTaskSubItemAsync(TaskSubItemDtoForCreate dto)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                // Aynı TaskAssignmentId ve Order değerine sahip bir kayıt var mı kontrol ediyoruz
                var exists = await _repository.TaskSubItems
                .FindByCondition(x =>
                    x.TaskAssignmentId == dto.TaskAssignmentId &&
                    x.Order == dto.Order,
                    false)
                    .AnyAsync();

                if (exists)
                    throw new Exception("Bu sırada zaten bir alt görev var");
                //yukarıdaki kontrolü geçersek yeni alt görevi oluşturuyoruz

                var entity = _mapper.Map<TaskSubItem>(dto);
                entity.CreatedAt = DateTime.UtcNow; // Oluşturulma zamanını ayarlıyoruz

                _repository.TaskSubItems.Create(entity);
                await _repository.SaveAsync();

                await _repository.CommitTransactionAsync();

                return _mapper.Map<TaskSubItemDtoForCreate>(entity);
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransactionAsync();
                _logger.LogError("TaskSubItem oluşturulurken hata oluştu.",ex);
                throw;
            }
        }

        public async Task UpdateTaskSubItemAsync(TaskSubItemDtoForUpdate dto)
        {
            var entity = await _repository.TaskSubItems
                .FindByCondition(tsi => tsi.Id == dto.Id, trackChanges: true)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new Exception("alt görev bulunamadı");

            // Varsa güncellenmesi gereken özellikler
            _mapper.Map(dto, entity);

            // Eğer tamamlandıysa, tamamlanma zamanını ayarlıyoruz
            if (dto.IsCompleted && entity.CompletedAt == null)
            {
                entity.CompletedAt = DateTime.UtcNow;

            }

            if (!dto.IsCompleted)
            {
                entity.CompletedAt = null;
            }

            await _repository.SaveAsync();
      
        }

        public async Task DeleteTaskSubItemAsync(int id)
        {
            var entity = await _repository.TaskSubItems
                .FindByCondition(tsi => tsi.Id == id, trackChanges: false)
                .SingleOrDefaultAsync();

            if (entity != null)
            {
                _repository.TaskSubItems.Delete(entity);
                await _repository.SaveAsync();
            }
            else
            {
                throw new Exception("TaskSubItem bulunamadı");
            }
        }

        public async Task<List<TaskSubItemDtoForRead>> GetTaskSubItemsByAssignmentId(int id)
        {
            var taskSubItems = await _repository.TaskSubItems
                .FindByCondition(tsi => tsi.TaskAssignmentId == id, trackChanges: false)
                .Include(tsi => tsi.TaskAssignment) // TaskAssignment ilişkisini dahil ediyoruz
                .ToListAsync();

            return _mapper.Map<List<TaskSubItemDtoForRead>>(taskSubItems);
        }

        public async Task UpdateTaskSubItemToggleAsync(TaskSubItemDtoForUpdate dto)
        {
            var entity = await _repository.TaskSubItems
                .FindByCondition(tsi => tsi.Id == dto.Id, trackChanges: true)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new Exception("alt görev bulunamadı");

            //ordera göre önceki alt görevlerin tamamlanma durumu
            if (dto.IsCompleted)
            {
                var canComplete =
                    await _repository.TaskSubItems
                        .ArePreviousOrdersCompleted(
                            entity.TaskAssignmentId,
                            entity.Order);

                if (!canComplete)
                    throw new Exception(
                        "Önceki alt görevler tamamlanmadan bu görev tamamlanamaz");
            }

            entity.IsCompleted = dto.IsCompleted;
            entity.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null;

            await _repository.SaveAsync();

            //üst görev kontrol
            var allCompleted =
                await _repository.TaskSubItems
                .AreAllCompletedbyAssignmentIdAsync(entity.TaskAssignmentId);

            var taskAssignment = await _repository.TaskAssignments.FindByCondition(x => x.Id == entity.TaskAssignmentId, trackChanges: true)
                .SingleAsync();

            if (allCompleted)
            {
                taskAssignment.IsCompleted = true;
                taskAssignment.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                taskAssignment.IsCompleted = false;
                taskAssignment.CompletedAt = null;
            }

            await _repository.SaveAsync();
        }
    }

}
