using SchoolOfRock.Infraestructure.Identity;

namespace SchoolOfRock.Infraestructure.Services
{
    public interface ITokenGenerator
    {
        string GerarToken(ApplicationUser user);
    }
}