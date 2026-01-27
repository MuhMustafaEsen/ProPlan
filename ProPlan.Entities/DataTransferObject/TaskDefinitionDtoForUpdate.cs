using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskDefinitionDtoForUpdate
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

}
