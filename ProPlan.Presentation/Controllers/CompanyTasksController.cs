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
    [Route("api/company-tasks")]
    public class CompanyTasksController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompanyTasksController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.CompanyTasks.GetAllCompanyTasksAsync();

            return Ok(GenericApiResponse<IEnumerable<CompanyTaskDtoForRead>>.SuccessResponse(result,"şirket görevleri listelendi"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.CompanyTasks.GetCompanyTaskByIdAsync(id);
            if (result == null)
                return NotFound(GenericApiResponse<CompanyTaskDtoForRead>.FailResponse("şirket görevi bulunamadı."));

            return Ok(GenericApiResponse<CompanyTaskDtoForRead>.SuccessResponse(result,"şirlet görev bilgileri"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyTaskDtoForCreate dto)
        {
            var result = await _service.CompanyTasks.CreateCompanyTaskAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                GenericApiResponse<CompanyTaskDtoForRead>.SuccessResponse(result, "şirket görevi oluşturuldu."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] CompanyTaskDtoForUpdate dto)
        {
            if (id != dto.Id)
                return BadRequest(GenericApiResponse<string>.FailResponse("id bulunamadı"));

            await _service.CompanyTasks.UpdateCompanyTaskAsync(dto);
            return Ok(GenericApiResponse<string>.SuccessResponse("OK","şirket görevi güncellendi."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.CompanyTasks.DeleteCompanyTaskAsync(id);
            return Ok(GenericApiResponse<string>.SuccessResponse("OK","şirket görevi başarı ile silindi."));
        }
    }

}
