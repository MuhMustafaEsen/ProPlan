using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.Models;
using ProPlan.Repositories.ModelConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.Contracts
{
    public class RepositoryContext:DbContext
    {
        public RepositoryContext(DbContextOptions options): base(options)
        {           
        }

        public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
        public DbSet<User> Users => Set<User>();
        public DbSet<CompanyTask> CompanyTasks => Set<CompanyTask>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<TaskDefinition> TaskDefinitions => Set<TaskDefinition>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<TaskSubItem> TaskSubItems => Set<TaskSubItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new CompanyTaskConfig());
            modelBuilder.ApplyConfiguration(new TaskSubItemConfig());
 
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            SetCreatedAtForNewEntities();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedAtForNewEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedAtForNewEntities()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                var createdAtProperty = entry.Entity.GetType().GetProperty("CreatedAt");
                if (createdAtProperty != null && createdAtProperty.PropertyType == typeof(DateTime))
                {
                    var currentValue = createdAtProperty.GetValue(entry.Entity);
                    if (currentValue == null || (DateTime)currentValue == default(DateTime))
                    {
                        createdAtProperty.SetValue(entry.Entity, DateTime.UtcNow);
                    }
                }
            }
        }
    }
}
