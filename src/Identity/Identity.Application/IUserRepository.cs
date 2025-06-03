using Identity.Domain.Entity;
using SchoolOfRock.Shared.Repository;

namespace Identity.Application
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName);
    }
}