using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class CompanyProgressDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public double CompletionRate { get; set; } // yüzde
    }
}
