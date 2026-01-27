using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskAssignmentDtoForRead
    {
        public int Id { get; set; }

        public int CompanyTaskId { get; set; }
        public string? CompanyName { get; set; }
        public string? TaskName { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }

        public DateTime TaskDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Alt görevler için 10.01.2026 eklendi TaskSubItemProfile için eklendi

        public string TaskTitle { get; set; } = null!;
        public string TaskDescription { get; set; } = null!;
    }

}
