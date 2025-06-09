using FluentValidation.TestHelper;
using Identity.Application.Command;
using SchoolOfRock.Tests.Identity.Validators;
using Xunit;

namespace SchoolOfRock.Tests.Identity.Application
{
    public class RegisterCommandValidatorTests
    {
        private readonly Validators.RegisterCommandValidatorTests _validator;

        public RegisterCommandValidatorTests()
        {
            _validator = new Validators.RegisterCommandValidatorTests();
        }

        [Fact]
        public void Deve_Ter_Erro_Quando_Nome_For_Vazio()
        {
            var command = new RegisterCommand { Name = "", Login = "email@valido.com", Password = "password" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Deve_Ter_Erro_Quando_Email_For_Invalido()
        {
            var command = new RegisterCommand { Name = "Nome", Login = "email-invalido", Password = "password" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Login);
        }
    }
}
