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
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenGenerator tokenGenerator, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
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
                Receivers = new List<string> { email, }.AsReadOnly(),
                Subject = "Forgot password",
                Body =
                    $"This is your otp: {otp}, expires in {expiryTimeSpan.Minutes} minutes.",
            };

            _ = Task.Run(() => _emailService.SendEmailAsync(mailBody));
        }

        public async Task<bool> VerifyOtpAsync(VerifyOtpDto verifyDto)
        {
            var result = await _unitOfWork.UserRepository.GetAsync(
                s => s.Email == verifyDto.Email.Trim(),
                tracked: false
            );

            //var isOtp = result.OtpCode.Equals(verifyDto.Otp.Trim());
            //if (!isOtp)
            //{
            //    return false;
            //}

            //var expiration = result.OtpCodeExpiration!.Value;
            //var now = DateTime.Now;

            //if (now.CompareTo(expiration) > 0)
            //{
            //    return false;
            //}

            return true;
        }
    }
}
