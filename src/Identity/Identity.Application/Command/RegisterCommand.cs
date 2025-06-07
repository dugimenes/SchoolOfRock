using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Command
{
    public class RegisterCommand : IRequest<Guid>
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public required string Login { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}