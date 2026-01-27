using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ProPlan.Entities.DataTransferObject;
using ProPlan.Repositories.Abstract;
using ProPlan.Services.Abstracts;
using ProPlan.Services.Auth.Abstract;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Services.Auth.Contract
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repository;
        private readonly ITokenService _tokenService;
        private readonly ILoggerService _logger;
        public AuthService(IRepositoryManager repository, ITokenService tokenService, ILoggerService logger)
        {
            _repository = repository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repository.Users
                .FindByCondition(u => u.Email == dto.Email, true)
                .SingleOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            refreshToken.UserId = user.Id;

            _repository.RefreshTokens.Create(refreshToken);
            await _repository.SaveAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var refreshToken =
        await _repository.RefreshTokens.GetValidTokenAsync(token);

            if (refreshToken == null) { 
                _logger.LogError("Invalid refresh token: " + token);
                throw new UnauthorizedAccessException("Invalid refresh token");
            }
            refreshToken.IsRevoked = true;

            var newRefreshToken = _tokenService.CreateRefreshToken();
            newRefreshToken.UserId = refreshToken.UserId;

            _repository.RefreshTokens.Create(newRefreshToken);

            var newAccessToken =
                _tokenService.CreateAccessToken(refreshToken.User);

            await _repository.SaveAsync();

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
        public async Task LogoutAsync(string refreshToken)
        {
            var tokenEntity =
                await _repository.RefreshTokens
                    .FindByCondition(
                        x => x.Token == refreshToken && !x.IsRevoked,
                        trackChanges: true)
                    .SingleOrDefaultAsync();

            if (tokenEntity == null)
                return; // güvenlik: token yoksa bile sessizce geç

            tokenEntity.IsRevoked = true;
            await _repository.SaveAsync();
        }
    }

}
