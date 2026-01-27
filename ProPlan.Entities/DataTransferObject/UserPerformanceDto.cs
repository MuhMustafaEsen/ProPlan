using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class UserPerformanceDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public int Total { get; set; }
        public int Completed { get; set; }
        public double Rate { get; set; } // yüzde
    }
}
