using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.GenericResponseModels;
using ProPlan.Entities.JwtCustomClaims;
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
    [Route("api/task-assignments")]
    public class TaskAssignmentsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TaskAssignmentsController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        //Role eklenince aç şimdilik kapalı dursun
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.TaskAssignments
                .GetAllTaskAssignmentsAsync();

            return Ok(GenericApiResponse<IEnumerable<TaskAssignmentDtoForRead>>
                .SuccessResponse(result,"tümü listelendi"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.TaskAssignments.GetTaskAssignmentByIdAsync(id);

            if (result == null)
                return NotFound(GenericApiResponse<TaskAssignmentDtoForRead>
                    .FailResponse("şirket bilgisi bulunamadı"));

            return Ok(GenericApiResponse<TaskAssignmentDtoForRead>
                .SuccessResponse(result,"görev tanımı getirildi."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskAssignmentDtoForCreate dto)
        {
            /*
            var exists = await _service.TaskAssignments.AnyAsync(x =>
                x.CompanyTaskId == dto.CompanyTaskId &&
                x.UserId == dto.UserId &&
                x.TaskDate.Date == dto.TaskDate.Date);

            if (exists)
                return BadRequest("Bu görev zaten atanmış.");
            */
            var assignment = new TaskAssignmentDtoForCreate
            {
                CompanyTaskId = dto.CompanyTaskId,
                UserId = dto.UserId,
                TaskDate = dto.TaskDate,
                //IsCompleted = false
            };

            var created = await _service.TaskAssignments.CreateTaskAssignmentAsync(assignment);

            //await _service.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id }, GenericApiResponse<TaskAssignmentDtoForRead>
                .SuccessResponse(created, "görev ataması yapıldı"));

            /*
            var created = await _service.TaskAssignments.CreateTaskAssignmentAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },GenericApiResponse<TaskAssignmentDtoForRead>
                .SuccessResponse(created, "görev ataması yapıldı"));
            */
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] TaskAssignmentDtoForUpdate dto)
        {
            if (id != dto.Id)
                return BadRequest(GenericApiResponse<string>
                    .FailResponse("id eşleşmedi."));

            await _service.TaskAssignments.UpdateTaskAssignmentAsync(dto);

            return Ok(GenericApiResponse<string>
                .SuccessResponse("OK","Başarılı şekilde güncellendi"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.TaskAssignments.DeleteTaskAssignmentAsync(id);

            return Ok(GenericApiResponse<string>
                .SuccessResponse("OK","görev sistemden silindi"));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyTasks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.TaskAssignments
                .GetMyTaskAssignmentsAsync(userId);

            return Ok(GenericApiResponse<IEnumerable<TaskAssignmentDtoForRead>>
                .SuccessResponse(result, "Kullanıcının görevleri başarıyla listelendi"));
            /*
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _service.TaskAssignments
                .GetMyTaskAssignmentsAsync(userId);

            return Ok(GenericApiResponse<IEnumerable<TaskAssignmentDtoForRead>>
                .SuccessResponse(result, "Usermy görevlerimin tümü listelendi"));
            */
        }

        //[HttpPut("{id}/complete")]
        //postu put yaptım
        [HttpPost("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await _service.TaskAssignments
                .CompleteTaskAssignmentAsync(id, userId);

            return Ok(GenericApiResponse<string>
                .SuccessResponse("OK","Başarıyla tamalandı"));
        }

        [HttpPost("monthly")]
        public async Task<IActionResult> GetMonthlyTasks([FromBody] MonthlyTaskFilterDto filter)
        {
            var result = await _service.TaskAssignments
                .GetMonthlyTasksAsync(filter);

            return Ok(GenericApiResponse<IEnumerable<MonthlyTaskAssignmentDto>>.SuccessResponse(result,"aylık görevler listelendi"));
        }
    }
}
