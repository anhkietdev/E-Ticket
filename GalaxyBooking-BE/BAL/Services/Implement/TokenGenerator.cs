using BAL.Services.Interface;
using System.Security.Cryptography;
using System.Text;

namespace BAL.Services.Implement
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly TokenSettings _tokenSettings;

        public TokenGenerator(TokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public (string, TimeSpan) GenerateOtp(int truncationLevel = 6)
        {
            var builder = GenerateRandomCode(truncationLevel);

            return (builder.ToString(), new TimeSpan(0, _tokenSettings.OtpExpiryMinutes, 0));
        }

        private static StringBuilder GenerateRandomCode(int truncationLevel)
        {
            var alphanumeric = @"0123456789AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

            var builder = new StringBuilder();

            for (int i = 0; i < truncationLevel; ++i)
            {
                var rand = RandomNumberGenerator.GetInt32(alphanumeric.Length);

                builder.Append(alphanumeric[rand]);
            }

            return builder;
        }
    }
}
