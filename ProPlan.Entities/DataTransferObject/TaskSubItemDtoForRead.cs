using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskSubItemDtoForRead
    {
        public int Id { get; set; }

        public int TaskAssignmentId { get; set; }
        public DateTime TaskDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Order { get; set; }

        // İlişkili diğer alanlar

        // TaskAssignment içinden gelen alanlar
        public int UserId { get; set; }
        public int CompanyTaskId { get; set; }

        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;

    }
}
