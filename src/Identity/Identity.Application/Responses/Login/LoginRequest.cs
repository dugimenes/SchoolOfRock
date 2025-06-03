using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Responses.Login
{
    public class LoginRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public required string Login { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}