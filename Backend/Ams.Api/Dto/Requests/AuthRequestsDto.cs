using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ams.Api.Dto.Requests
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Name { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
    public class ChangePasswordDto
    {
        public string UserId { get; set; } = "";
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
