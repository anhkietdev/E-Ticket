using BAL.DTOs.Authentication;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.Extensions.Configuration;

namespace BAL.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public Task ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(
                u => u.Email == loginDto.Email && u.Password == loginDto.Password,
                tracked: false
            );

            if (user == null)
            {
                return new AuthenResultDto
                {
                    IsSuccess = false,
                };
            }
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            string token = JwtGenerator.GenerateToken(user, secretKey, 1000000, issuer, audience);
            return new AuthenResultDto
            {
                IsSuccess = true,
                Token = token,
            };
        }

        public async Task<AuthenResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var newUser = new User
            {
                FullName = registerDto.Fullname,
                Email = registerDto.Email,
                Password = registerDto.Password,
            };

            var result = _unitOfWork.UserRepository.AddAsync(newUser);

            await _unitOfWork.SaveAsync();

            if (newUser == null)
            {
                return new AuthenResultDto
                {
                    IsSuccess = false,
                };
            }

            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            string token = JwtGenerator.GenerateToken(newUser, secretKey, 1000000, issuer, audience);

            return new AuthenResultDto
            {
                IsSuccess = true,
                Token = token,
            };
        }

        public Task<bool> VerifyOtpAsync(VerifyOtpDto verifyDto)
        {
            throw new NotImplementedException();
        }
    }
}
