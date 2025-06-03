using Identity.Domain.AggregateModel;
using SchoolOfRock.Shared.Repository;

namespace Identity.Infra.Repository
{
    public interface IUserRepository : IRepository<IdentityDbContext, ApplicationUser>
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName);
    }
}