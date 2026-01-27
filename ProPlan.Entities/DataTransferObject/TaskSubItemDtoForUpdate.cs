using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskSubItemDtoForUpdate
    {
        public int Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public int? Order { get; set; }
    }
}
