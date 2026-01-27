using Microsoft.AspNetCore.Authorization;
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
    [Route("api/companies")]
    
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _service.Companies.GetAllCompaniesAsync();
            
            return Ok(GenericApiResponse<IEnumerable<CompanyDtoForRead>>.SuccessResponse(companies, "Companies bilgisi listelendi"));
            //Generic response öncesi
            //return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _service.Companies.GetCompanyByIdAsync(id);

            return Ok(GenericApiResponse<CompanyDtoForRead>
            .SuccessResponse(company, "şirket bilgisi başarıyla getirildi."));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDtoForCreate companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericApiResponse<object>.FailResponse("Geçersiz model.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var createdCompany = await _service.Companies.CreateCompanyAsync(companyDto);

            return CreatedAtAction(
                nameof(GetCompanyById),
                new { id = createdCompany.Id },
                GenericApiResponse<CompanyDtoForRead>
                .SuccessResponse(createdCompany, "Firma oluşturuldu."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyDtoForUpdate companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericApiResponse<object>.FailResponse("Geçersiz model.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            if (id != companyDto.Id)
                return BadRequest(GenericApiResponse<string>.FailResponse("Id eşleşmedi"));

            await _service.Companies.UpdateCompanyAsync(companyDto);

            return Ok(GenericApiResponse<string>
            .SuccessResponse("OK", "şirket başarıyla güncellendi"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _service.Companies.DeleteCompanyAsync(id);

            return Ok(GenericApiResponse<string>
            .SuccessResponse("OK", "Şirket başarıyla silindi."));
        }
    }
}
