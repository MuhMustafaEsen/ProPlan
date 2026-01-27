using Microsoft.AspNetCore.Mvc;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.GenericResponseModels;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Presentation.Controllers
{
    [ApiController]
    [Route("api/Dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IServiceManager _service;

        public DashboardController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("company-progress")]
        public async Task<IActionResult> GetCompanyProgress()
        {
            var data = await _service.Dashboards.GetCompanyProgressAsync();
            return Ok(GenericApiResponse<IEnumerable<CompanyProgressDto>>.SuccessResponse(data,"Şirket işleri raporlandı"));
        }

        [HttpGet("monthly-completion")]
        public async Task<IActionResult> GetMonthlyCompletion()
        {
            var data = await _service.Dashboards.GetMonthlyCompletionAsync();
            return Ok(GenericApiResponse<IEnumerable<MonthlyCompletionDto>>.SuccessResponse(data, "aylık tamamlanan işler raporlandı"));
        }

        [HttpGet("user-performance")]
        public async Task<IActionResult> GetUserPerformance()
        {
            var data = await _service.Dashboards.GetUserPerformanceAsync();
            return Ok(GenericApiResponse<IEnumerable<UserPerformanceDto>>.SuccessResponse(data, "Şirket çalışanları raporlandı"));
        }
    }

}
