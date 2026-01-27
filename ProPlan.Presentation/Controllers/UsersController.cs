using Microsoft.AspNetCore.Mvc;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.GenericResponseModels;
using ProPlan.Entities.Models;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Presentation.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _service;
        public UsersController(IServiceManager service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.Users.GetAllUsersAsync();

            return Ok(GenericApiResponse<IEnumerable<UserDtoForRead>>.SuccessResponse(users,"kullanıcılar başırılı olarak getirildi"));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _service.Users.GetUserByIdAsync(id);

            return Ok(GenericApiResponse<UserDtoForRead>.SuccessResponse(user,"kullanıcı başarılı olarak getirildi."));
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDtoForCreate userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericApiResponse<object>.FailResponse("Geçersiz model.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var createdUser = await _service.Users.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, 
                GenericApiResponse<UserDtoForRead>
                .SuccessResponse(createdUser, "Kullanıcı oluşturuldu."));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDtoForUpdate userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GenericApiResponse<object>.FailResponse("Geçersiz model.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            if (id != userDto.Id)
            {
                return BadRequest(GenericApiResponse<string>
                    .FailResponse("ID eşleşmedi"));
            }

            await _service.Users.UpdateUserAsync(userDto);
            
            return Ok(GenericApiResponse<string>
                .SuccessResponse("Ok", "kullanıcı başırılı olarak güncellendi"));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _service.Users.DeleteUserAsync(id);

            return Ok(GenericApiResponse<string>.SuccessResponse("Ok", "kullanıcı başarılı olarak silindi"));
        }   
    }
}
