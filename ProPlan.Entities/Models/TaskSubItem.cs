using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Models
{
    public class TaskSubItem
    {
        public int Id { get; set; }
        public int TaskAssignmentId { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Order { get; set; } // görev sıralaması için Unique olursa iyi olur
        public DateTime CreatedAt { get; set; }

        public TaskAssignment TaskAssignment { get; set; } = null!;
    }
}
