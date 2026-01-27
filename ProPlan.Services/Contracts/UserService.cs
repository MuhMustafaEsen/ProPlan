using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Entities.Exceptions;
using ProPlan.Entities.Models;
using ProPlan.Repositories.Abstract;
using ProPlan.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Contracts
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public UserService(IRepositoryManager repository, IMapper mapper, ILoggerService logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserDtoForRead>> GetAllUsersAsync()
        {
            var users = await _repository.Users
                .FindAll(trackChanges: false)
                .ToListAsync();

            return _mapper.Map<List<UserDtoForRead>>(users);
        }

        public async Task<UserDtoForRead?> GetUserByIdAsync(int id)
        {  
            var user = await _repository.Users.GetByIdAsync(id, trackChanges: false);

            if (user == null)
                throw new NotFoundException(nameof(Entities.Models.User), id);

            return _mapper.Map<UserDtoForRead>(user);
        }
        public async Task<UserDtoForRead> CreateUserAsync(UserDtoForCreate userDto)
        {
            _logger.LogInfo($"Starting user creation process for email: {userDto.Email}");

            var existingUser = await _repository.Users.GetByEmailAsync(userDto.Email, trackChanges: false);
            
            if (existingUser != null)
            {
                _logger.LogWarn($"User creation failed: Email {userDto.Email} already exists.");
                throw new ConflictException("Kullanıcı", userDto.Email);
            }

            await _repository.BeginTransactionAsync();

            try
            {
                var newUser = _mapper.Map<User>(userDto);
                newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.PasswordH);
                newUser.CreatedAt = DateTime.UtcNow;

                _repository.Users.Create(newUser);
                await _repository.SaveAsync();
                await _repository.CommitTransactionAsync();

                _logger.LogInfo($"User başarlı bir şekilde oluşturuldu. User ID: {newUser.Id}");
                    
                return _mapper.Map<UserDtoForRead>(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"kullanıcı oluşturulurken hata oldu. Rolling back transaction. Error: {ex.Message}", ex);
                await _repository.RollbackTransactionAsync();
                
                throw;
            }
        }

        public async Task UpdateUserAsync(UserDtoForUpdate userDto)
        {
            var user = await _repository.Users.GetByIdAsync(userDto.Id, trackChanges: true);

            if (user == null)
                throw new NotFoundException(nameof(Entities.Models.User), userDto.Id);

            _mapper.Map(userDto, user);
            await _repository.SaveAsync();

            _logger.LogInfo($"User updated successfully. User ID: {userDto.Id}");
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _repository.Users.GetByIdAsync(id, trackChanges: true);

            if (user == null)
                throw new NotFoundException(nameof(Entities.Models.User), id);

            _repository.Users.Delete(user);
            await _repository.SaveAsync();

            _logger.LogInfo($"kullanıcı başarılı bir şekilde silindi. User ID: {id}");
        }
    }
}
