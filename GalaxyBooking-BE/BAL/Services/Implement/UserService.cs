using BAL.DTOs.Authentication;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http; 

using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BAL.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor; // Thêm để lấy thông tin từ token

        public UserService(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            ITokenGenerator tokenGenerator,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor // Thêm vào constructor
        )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthenResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(
                u => u.Email == loginDto.Email,
                tracked: false
            );

            if (user == null)
            {
                return new AuthenResultDto
                {
                    IsSuccess = false,
                    RefreshToken = "Wrong email"
                };
            }

            var hashPass = HashPass.HashWithSHA256(loginDto.Password);

            if (hashPass != user.Password)
            {
                return new AuthenResultDto
                {
                    IsSuccess = false,
                    RefreshToken = "Wrong password"
                };
            }

            var secretKey = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expireTime = int.Parse(_configuration["JwtSettings:ExpiryMinutes"]);
            string token = JwtGenerator.GenerateToken(user, secretKey, expireTime, issuer, audience);
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
                Password = HashPass.HashWithSHA256(registerDto.Password),
            };

            var check = _unitOfWork.UserRepository.GetAsync(
                u => u.Email == registerDto.Email,
                tracked: false
            );

            if (check != null) { 
                throw new Exception("Email already exists");
            }

            await _unitOfWork.UserRepository.AddAsync(newUser);
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

        public async Task ForgotPasswordAsync(string email)
        {
            var result = await _unitOfWork.UserRepository.GetAsync(
                s => s.Email == email.Trim(),
                tracked: true
            );

            var (otp, expiryTimeSpan) = _tokenGenerator.GenerateOtp();

            await _unitOfWork.SaveAsync();

            var mailBody = new MailDto
            {
                Receivers = new List<string> { email }.AsReadOnly(),
                Subject = "Forgot password",
                Body = $"This is your otp: {otp}, expires in {expiryTimeSpan.Minutes} minutes.",
            };

            _ = Task.Run(() => _emailService.SendEmailAsync(mailBody));
        }

        public async Task<bool> VerifyOtpAsync(VerifyOtpDto verifyDto)
        {
            var result = await _unitOfWork.UserRepository.GetAsync(
                s => s.Email == verifyDto.Email.Trim(),
                tracked: false
            );


            return true;
        }


        public Task<ICollection<User>> GetAllAsync()
        {
            var users = _unitOfWork.UserRepository.GetAllAsync(
                tracked: false
            );
            return users;
        }
        public async Task<User> GetProfileAsync()
        {
            // Thử tìm claim nameid thủ công
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("nameid") ??
                              _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                // Log tất cả claims để debug
                Console.WriteLine("Claims in HttpContext.User:");
                foreach (var claim in _httpContextAccessor.HttpContext?.User.Claims ?? Enumerable.Empty<Claim>())
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
                throw new UnauthorizedAccessException("Không tìm thấy thông tin người dùng trong token.");
            }

            Console.WriteLine($"Found userIdClaim: {userIdClaim.Value}");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                Console.WriteLine($"Invalid Guid format for userId: {userIdClaim.Value}");
                throw new UnauthorizedAccessException("ID người dùng trong token không hợp lệ.");
            }

            var user = await _unitOfWork.UserRepository.GetAsync(
                u => u.Id == userId,
                tracked: false
            );

            if (user == null)
            {
                Console.WriteLine($"User not found with Id: {userId}");
                throw new KeyNotFoundException("Không tìm thấy người dùng.");
            }

            return user;

        }
    }
}