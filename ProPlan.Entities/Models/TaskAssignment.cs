using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }

        public int CompanyTaskId { get; set; }
        public CompanyTask CompanyTask { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime TaskDate { get; set; } // Takvim günü
        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }
        //taskSubItem ilişkisi için eklendi.
        public ICollection<TaskSubItem> SubTasks { get; set; }
        = new List<TaskSubItem>();
    }

}
