using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Domain.Entity;
using SchoolOfRock.Infraestructure.Identity;

namespace SchoolOfRock.Infraestructure.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext){}
        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            return Task.FromResult(result == PasswordVerificationResult.Success);
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Cria o ApplicationUser
                var result = await base.CreateAsync(entity);

                // Adiciona o Usuario correspondente
                var appUser = new Aluno(userName!, entity.Email!);
                
                if (Guid.TryParse(entity.Id, out var guid))
                {
                    appUser.Id = guid;
                }
                else
                {
                    throw new Exception("ID do usuário inválido.");
                }


                await _dbContext.Alunos.AddAsync(appUser);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}