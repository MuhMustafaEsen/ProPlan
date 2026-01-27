using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Models
{
    public class TaskDefinition
    {
        public int Id { get; set; }

        public string TaskName { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CompanyTask> CompanyTasks { get; set; } = new List<CompanyTask>();
    }

}
