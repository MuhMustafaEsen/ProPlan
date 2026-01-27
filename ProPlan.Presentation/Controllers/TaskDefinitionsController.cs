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
    [Route("api/task-definitions")]
    public class TaskDefinitionsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TaskDefinitionsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.TaskDefinitions.GetAllTaskDefinitionsAsync();

            return Ok(GenericApiResponse<IEnumerable<TaskDefinitionDtoForRead>>
                .SuccessResponse(result,"görev tanımları listelendi."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.TaskDefinitions.GetTaskDefinitionByIdAsync(id);
            if (result == null)
                return NotFound(GenericApiResponse<TaskDefinitionDtoForRead>
                    .FailResponse("görev tanımı bulunamdı."));

            return Ok(GenericApiResponse<TaskDefinitionDtoForRead>.SuccessResponse(result,"Görev başarı ile getirildi"));
        }

        [HttpPost]
        //bu controllerda hata olabilir incele
        public async Task<IActionResult> Create([FromBody] TaskDefinitionDtoForCreate dto)
        {
            var created = await _service.TaskDefinitions.CreateTaskDefinitionAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                GenericApiResponse<TaskDefinitionDtoForRead>
                .SuccessResponse(created, "Görev tanımı başarı ile oluşturuldu."));       
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] TaskDefinitionDtoForUpdate dto)
        {
            if (id != dto.Id)
                return BadRequest(GenericApiResponse<string>.FailResponse("Görev tanımı bulunamdı"));

            await _service.TaskDefinitions.UpdateTaskDefinitionAsync(dto);

            return Ok(GenericApiResponse<string>.SuccessResponse("Ok","görev başarı ile güncellendi."));
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.TaskDefinitions.DeleteTaskDefinitionAsync(id);
            return NoContent();
        }
    }

}
