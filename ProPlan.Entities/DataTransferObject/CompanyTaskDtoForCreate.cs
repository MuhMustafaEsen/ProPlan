using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class CompanyTaskDtoForCreate
    {
        public int CompanyId { get; set; }
        public int TaskDefinitionId { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
    }

}
