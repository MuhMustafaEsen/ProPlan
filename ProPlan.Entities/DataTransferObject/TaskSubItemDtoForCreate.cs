using ProPlan.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskSubItemDtoForCreate
    {
        public int id { get; set; }
        public int TaskAssignmentId { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Order { get; set; } // Görev sıralaması

    }
}
