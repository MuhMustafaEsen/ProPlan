using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Models
{
    public class CompanyTask
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public int TaskDefinitionId { get; set; }
        public TaskDefinition TaskDefinition { get; set; } = null!;

        public int Year { get; set; }
        public int Month { get; set; } // 1–12

        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    }

}
