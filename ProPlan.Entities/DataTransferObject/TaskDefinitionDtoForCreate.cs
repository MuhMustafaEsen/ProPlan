using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class TaskDefinitionDtoForCreate
    {      
        public string TaskName { get; set; } = null!;
        public string? Description { get; set; }
    }

}
