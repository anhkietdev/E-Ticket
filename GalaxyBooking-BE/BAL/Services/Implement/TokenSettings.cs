using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implement
{
    public class TokenSettings
    {
        public const string SectionName = "TokenSettings";
        public int OtpExpiryMinutes { get; init; }
        public int RefreshTokenExpiryMinutes { get; init; }
    }
}
