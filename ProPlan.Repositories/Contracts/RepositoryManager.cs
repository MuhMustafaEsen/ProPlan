using Microsoft.EntityFrameworkCore.Storage;
using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private IDbContextTransaction? _transaction;

        private readonly Lazy<IUserRepository> _usersRepository;
        private readonly Lazy<ICompanyRepository> _companyRepository;
        private readonly Lazy<ICompanyTaskRepository> _companyTasks;
        private readonly Lazy<ITaskDefinitionRepository> _taskDefinitions;
        private readonly Lazy<ITaskAssignmentRepository> _taskAssignments;
        private readonly Lazy<IDashboardRepository> _dashboard;
        private readonly Lazy<ITaskSubItemRepository> _taskSubItems;
        private readonly Lazy<IRefreshTokenRepository> _refreshTokens;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _usersRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
            _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(_context));
            _companyTasks = new Lazy<ICompanyTaskRepository>(() => new CompanyTaskRepository(_context));
            _taskDefinitions = new Lazy<ITaskDefinitionRepository>(() => new TaskDefinitionRepository(_context));
            _taskAssignments = new Lazy<ITaskAssignmentRepository>(() => new TaskAssignmentRepository(_context));
            _taskSubItems = new Lazy<ITaskSubItemRepository>(() => new TaskSubItemRepository(_context));
            _refreshTokens = new Lazy<IRefreshTokenRepository>(() => new RefreshTokenRepository(_context));
            _dashboard = new Lazy<IDashboardRepository>(() => new DashboardRepository(_context));
        }
        public IUserRepository Users => _usersRepository.Value;
        public ICompanyRepository Companies => _companyRepository.Value;
        public ICompanyTaskRepository CompanyTasks => _companyTasks.Value;
        public ITaskDefinitionRepository TaskDefinitions => _taskDefinitions.Value;
        public ITaskAssignmentRepository TaskAssignments => _taskAssignments.Value;
        public ITaskSubItemRepository TaskSubItems => _taskSubItems.Value;
        public IRefreshTokenRepository RefreshTokens => _refreshTokens.Value;
        public IDashboardRepository Dashboards => _dashboard.Value;
        

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
