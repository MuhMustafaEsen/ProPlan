using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskAssignmentDtoForCreate
    {
        public int CompanyTaskId { get; set; }
        public int UserId { get; set; }

        public DateTime TaskDate { get; set; }
    }

}
