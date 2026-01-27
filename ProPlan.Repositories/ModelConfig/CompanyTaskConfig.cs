using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.ModelConfig
{
    public class CompanyTaskConfig:IEntityTypeConfiguration<CompanyTask>
    {
        public void Configure(EntityTypeBuilder<CompanyTask> builder)
        {
            builder.HasIndex(x => new { x.CompanyId, x.TaskDefinitionId, x.Year, x.Month })
                .IsUnique();

        }
    }
}
