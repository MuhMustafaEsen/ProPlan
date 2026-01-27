using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class CompanyTaskDtoForRead
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }
        //Yeni eklenen aşağıda 1 satır
        public string CompanyName { get; set; }
        public int TaskDefinitionId { get; set; }
        //Yeni eklenen aşağıda 1 satır
        public string TaskName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
    }

}
