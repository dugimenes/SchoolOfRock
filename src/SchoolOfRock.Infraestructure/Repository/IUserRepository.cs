using SchoolOfRock.Infraestructure.Identity;

namespace SchoolOfRock.Infraestructure.Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName);
    }
}
