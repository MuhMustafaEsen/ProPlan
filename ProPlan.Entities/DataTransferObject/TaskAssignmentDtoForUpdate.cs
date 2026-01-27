using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskAssignmentDtoForUpdate
    {
        public int Id { get; set; }

        public int CompanyTaskId { get; set; }
        public int UserId { get; set; }

        public DateTime TaskDate { get; set; }
        public bool IsCompleted { get; set; }
    }

}
