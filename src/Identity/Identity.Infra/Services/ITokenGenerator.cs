using Identity.Domain.AggregateModel;

namespace Identity.Infra.Services
{
    public interface ITokenGenerator
    {
        Task<string> GerarToken(ApplicationUser user);
    }
}