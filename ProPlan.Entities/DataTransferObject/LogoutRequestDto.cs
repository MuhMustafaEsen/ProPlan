using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.DataTransferObject
{
    public class LogoutRequestDto
    {
        public string RefreshToken { get; set; } = null!;
    }

}
