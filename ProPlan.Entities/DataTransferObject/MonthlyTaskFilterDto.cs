using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class MonthlyTaskFilterDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int? CompanyId { get; set; }
        public int? UserId { get; set; }
    }
}
