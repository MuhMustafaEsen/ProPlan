using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.GenericResponseModels;
using ProPlan.Entities.Models;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Presentation.Controllers
{
    [ApiController]
    [Route("api/task-subitems")]
    public class TaskSubItemController:ControllerBase
    {
        private readonly IServiceManager _service;

        public TaskSubItemController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize] // Role eklenince açılabilir, şu an için bu endpoint herkes tarafından erişilebilir
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.TaskSubItems.GetAllTaskSubItemAsync();
            return Ok(GenericApiResponse<IEnumerable<TaskSubItemDtoForRead>>
                .SuccessResponse(result, "Tüm alt görevler listelendi"));
        }

        // GET api/task-subitems/{id}
        [HttpGet("assignment/{id}")]
        public async Task<IActionResult> GetByTaskSubItemsByAssignmentId(int id)
        {
            var result = await _service.TaskSubItems.GetTaskSubItemsByAssignmentId(id);

            if (result == null)
                return NotFound(GenericApiResponse<TaskSubItemDtoForRead>
                    .FailResponse("Görev alt öğleri bulunamadı"));

            return Ok(GenericApiResponse<IEnumerable<TaskSubItemDtoForRead>>
                .SuccessResponse(result, "Görev alt öğleri listelendi."));
        }
        // GET api/task-subitems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.TaskSubItems.GetTaskSubItemByIdAsync(id);

            if (result == null)
                return NotFound(GenericApiResponse<TaskSubItemDtoForRead>
                    .FailResponse("Görev alt öğesi bulunamadı"));

            return Ok(GenericApiResponse<TaskSubItemDtoForRead>
                .SuccessResponse(result, "Görev alt öğesi getirildi."));
        }

        // POST api/task-subitems
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskSubItemDtoForCreate dto)
        {
            if (dto == null)
                return BadRequest(GenericApiResponse<string>
                    .FailResponse("Geçersiz veri"));

            var created = await _service.TaskSubItems.CreateTaskSubItemAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.id }, GenericApiResponse<TaskSubItemDtoForCreate>
                .SuccessResponse(created, "Görev alt öğesi başarıyla oluşturuldu"));
        }
        // PUT api/task-subitems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskSubItemDtoForUpdate dto)
        {
            if (id != dto.Id)
                return BadRequest(GenericApiResponse<string>
                    .FailResponse("ID eşleşmiyor"));

            try
            {
                await _service.TaskSubItems.UpdateTaskSubItemAsync(dto);
                return Ok(GenericApiResponse<string>
                    .SuccessResponse("OK", "Görev alt öğesi başarıyla güncellendi"));
            }
            catch (Exception ex)
            {
                return NotFound(GenericApiResponse<string>
                    .FailResponse(ex.Message));
            }
        }
        // DELETE api/task-subitems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.TaskSubItems.DeleteTaskSubItemAsync(id);
                return Ok(GenericApiResponse<string>
                    .SuccessResponse("OK", "Görev alt öğesi başarıyla silindi"));
            }
            catch (Exception ex)
            {
                return NotFound(GenericApiResponse<string>
                    .FailResponse(ex.Message));
            }
        }
        //yeni eklenen metod: alt görevi tamamla
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteSubItem(int id)
        {
            try
            {
                var dto = new TaskSubItemDtoForUpdate
                {
                    Id = id,
                    IsCompleted = true
                };

                await _service.TaskSubItems.UpdateTaskSubItemToggleAsync(dto);

                return Ok(GenericApiResponse<string>
                    .SuccessResponse("OK", "Alt görev tamamlandı"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(GenericApiResponse<string>
                    .FailResponse("Bu alt görevi tamamlama yetkiniz yok"));
            }
            catch (Exception ex)
            {
                return BadRequest(GenericApiResponse<string>
                    .FailResponse(ex.Message));
            }
            /*
            var dto = new TaskSubItemDtoForUpdate
            {
                Id = id,
                IsCompleted = true
            };

            await _service.TaskSubItems.UpdateTaskSubItemToggleAsync(dto);

            return Ok(GenericApiResponse<string>
                .SuccessResponse("OK", "Alt görev tamamlandı"));
            */
        }


        //Aşağıdaki metodlar opsiyonel ve ihtiyaç halinde kullanılabilir SONRADAN EKLİYECEĞİM
        /*
        // POST api/task-subitems/my
        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyTaskSubItems()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.TaskSubItems.GetMyTaskSubItemsAsync(userId);

            return Ok(GenericApiResponse<IEnumerable<TaskSubItemDtoForRead>>
                .SuccessResponse(result, "Kullanıcının görev alt öğeleri başarıyla listelendi"));
        }
       
        // POST api/task-subitems/{id}/complete
        [HttpPost("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                await _service.TaskSubItems.CompleteTaskSubItemAsync(id, userId);
                return Ok(GenericApiResponse<string>
                    .SuccessResponse("OK", "Görev alt öğesi başarıyla tamamlandı"));
            }
            catch (Exception ex)
            {
                return NotFound(GenericApiResponse<string>
                    .FailResponse(ex.Message));
            }
        }
        
        // POST api/task-subitems/monthly
        [HttpPost("monthly")]
        public async Task<IActionResult> GetMonthlyTaskSubItems([FromBody] MonthlyTaskFilterDto filter)
        {
            var result = await _service.TaskSubItems.GetMonthlyTaskSubItemsAsync(filter);

            return Ok(GenericApiResponse<IEnumerable<MonthlyTaskSubItemDto>>
                .SuccessResponse(result, "Aylık görev alt öğeleri listelendi"));
        }
        */
    }
}
