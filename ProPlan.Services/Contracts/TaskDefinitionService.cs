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
    public class TaskDefinitionService : ITaskDefinitionService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public TaskDefinitionService(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TaskDefinitionDtoForRead>> GetAllTaskDefinitionsAsync()
        {
            var tasks = await _repository.TaskDefinitions
                .FindAll(trackChanges: false)
                .ToListAsync();

            return _mapper.Map<List<TaskDefinitionDtoForRead>>(tasks);
        }

        public async Task<TaskDefinitionDtoForRead?> GetTaskDefinitionByIdAsync(int id)
        {
            var task = await _repository.TaskDefinitions
                .FindByCondition(t => t.Id == id, false)
                .SingleOrDefaultAsync();

            return task == null
                ? null
                : _mapper.Map<TaskDefinitionDtoForRead>(task);
        }

        public async Task<TaskDefinitionDtoForRead> CreateTaskDefinitionAsync(
            TaskDefinitionDtoForCreate dto)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                var entity = _mapper.Map<TaskDefinition>(dto);
                entity.CreatedAt = DateTime.UtcNow;
                entity.IsActive = true;

                _repository.TaskDefinitions.Create(entity);
                await _repository.SaveAsync();

                await _repository.CommitTransactionAsync();

                return _mapper.Map<TaskDefinitionDtoForRead>(entity);
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateTaskDefinitionAsync(TaskDefinitionDtoForUpdate dto)
        {
            var entity = await _repository.TaskDefinitions
                .FindByCondition(t => t.Id == dto.Id, true)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new Exception("TaskDefinition not found");

            _mapper.Map(dto, entity);
            await _repository.SaveAsync();
        }

        public async Task DeleteTaskDefinitionAsync(int id)
        {
            var entity = await _repository.TaskDefinitions
                .FindByCondition(t => t.Id == id, true)
                .SingleOrDefaultAsync();

            if (entity != null)
            {
                _repository.TaskDefinitions.Delete(entity);
                await _repository.SaveAsync();
            }
        }
    }

}
