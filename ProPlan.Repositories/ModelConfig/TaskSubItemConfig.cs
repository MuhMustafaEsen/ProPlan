using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Repositories.ModelConfig
{
    public class TaskSubItemConfig : IEntityTypeConfiguration<TaskSubItem>
    {
        public void Configure(EntityTypeBuilder<TaskSubItem> builder)
        {
            builder.HasIndex(x => new { x.TaskAssignmentId, x.Order })
                .IsUnique();
        }
    }
}
