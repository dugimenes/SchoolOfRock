using Identity.Domain.AggregateModel;

namespace SchoolOfRock.Api.Services
{
    public interface ITokenGenerator
    {
        string GerarToken(ApplicationUser user);
    }
}