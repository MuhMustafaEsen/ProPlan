using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string CompanyName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CompanyTask> CompanyTasks { get; set; } = new List<CompanyTask>();
    }
}
