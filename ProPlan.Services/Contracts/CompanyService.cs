using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Exceptions;
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
    public class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CompanyService(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CompanyDtoForRead>> GetAllCompaniesAsync()
        {
            var companies = await _repository.Companies
                .FindAll(trackChanges: false)
                .ToListAsync();

            return _mapper.Map<List<CompanyDtoForRead>>(companies);
        }

        public async Task<CompanyDtoForRead?> GetCompanyByIdAsync(int id)
        {
            var company = await _repository.Companies.GetByIdAsync(id, trackChanges: false);

            if (company == null)
                throw new NotFoundException(nameof(Entities.Models.Company), id);

            return _mapper.Map<CompanyDtoForRead>(company);
        }

        public async Task<CompanyDtoForRead> CreateCompanyAsync(CompanyDtoForCreate companyDto)
        {
            _logger.LogInfo($"Starting company creation process for: {companyDto.CompanyName}");

            await _repository.BeginTransactionAsync();

            try
            {
                var company = _mapper.Map<Company>(companyDto);
                company.CreatedAt = DateTime.UtcNow;
                company.IsActive = true;

                _repository.Companies.Create(company);
                await _repository.SaveAsync();

                await _repository.CommitTransactionAsync();

                _logger.LogInfo($"Company creation completed successfully. Company ID: {company.Id}");

                return _mapper.Map<CompanyDtoForRead>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Company creation failed. Rolling back transaction. Error: {ex.Message}", ex);
                await _repository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateCompanyAsync(CompanyDtoForUpdate companyDto)
        {
            var company = await _repository.Companies.GetByIdAsync(companyDto.Id, trackChanges: true);

            if (company == null)
                throw new NotFoundException(nameof(Company), companyDto.Id);

            _mapper.Map(companyDto, company);

            await _repository.SaveAsync();

            _logger.LogInfo($"Company updated successfully. Company ID: {companyDto.Id}");
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _repository.Companies.GetByIdAsync(id, trackChanges: true);

            if (company == null)
                throw new NotFoundException(nameof(Entities.Models.Company), id);

            _repository.Companies.Delete(company);
            await _repository.SaveAsync();

            _logger.LogInfo($"Company deleted successfully. Company ID: {id}");
        }
    }

}
