using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class MonthlyTaskAssignmentDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string TaskName { get; set; } = null!;
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime PlannedDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
