using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implement
{
    public class EmailSettings
    {
        public static string SectionName { get; set; } = "EmailSettingNames";
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public string? SmtpClient { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
