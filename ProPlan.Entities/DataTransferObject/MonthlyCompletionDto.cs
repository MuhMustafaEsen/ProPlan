using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class MonthlyCompletionDto
    {
        public string Month { get; set; } = null!;
        public double Rate { get; set; } // yüzde
    }
}
