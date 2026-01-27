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
    public class CompanyTaskService : ICompanyTaskService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CompanyTaskService(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CompanyTaskDtoForRead>> GetAllCompanyTasksAsync()
        {
            var companyTasks = await _repository.CompanyTasks
                .FindAll(trackChanges: false)
                .Include(ct => ct.Company)
                .Include(ct => ct.TaskDefinition)
                .ToListAsync();

            return _mapper.Map<List<CompanyTaskDtoForRead>>(companyTasks);
        }

        public async Task<CompanyTaskDtoForRead?> GetCompanyTaskByIdAsync(int id)
        {
            var companyTask = await _repository.CompanyTasks
                .FindByCondition(ct => ct.Id == id, false)
                .SingleOrDefaultAsync();

            return companyTask == null
                ? null
                : _mapper.Map<CompanyTaskDtoForRead>(companyTask);
        }

        public async Task<CompanyTaskDtoForRead> CreateCompanyTaskAsync(
            CompanyTaskDtoForCreate dto)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                var entity = _mapper.Map<CompanyTask>(dto);

                _repository.CompanyTasks.Create(entity);
                await _repository.SaveAsync();

                await _repository.CommitTransactionAsync();

                return _mapper.Map<CompanyTaskDtoForRead>(entity);
            }
            catch
            {
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateCompanyTaskAsync(CompanyTaskDtoForUpdate dto)
        {
            var entity = await _repository.CompanyTasks
                .FindByCondition(ct => ct.Id == dto.Id, true)
                .SingleOrDefaultAsync();

            if (entity == null)
                throw new Exception("CompanyTask not found");

            _mapper.Map(dto, entity);
            await _repository.SaveAsync();
        }

        public async Task DeleteCompanyTaskAsync(int id)
        {
            var entity = await _repository.CompanyTasks
                .FindByCondition(ct => ct.Id == id, true)
                .SingleOrDefaultAsync();

            if (entity != null)
            {
                _repository.CompanyTasks.Delete(entity);
                await _repository.SaveAsync();
            }
        }
    }

}
