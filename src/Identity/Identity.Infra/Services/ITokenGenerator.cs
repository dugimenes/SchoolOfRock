using Identity.Domain.AggregateModel;

namespace Identity.Infra.Services
{
    public interface ITokenGenerator
    {
        string GerarToken(ApplicationUser user);
    }
}