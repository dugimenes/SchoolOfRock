using Identity.Domain.AggregateModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolOfRock.Shared.Repository;

namespace Identity.Infra.Repository
{
    public class UserRepository : Repository<IdentityDbContext, ApplicationUser>, IUserRepository
    {
        public UserRepository(IdentityDbContext dbContext) : base(dbContext){}
        
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

        public Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName)
        {
            throw new NotImplementedException();
            //TODO : Implementar criação do usuário com o ApplicationUser e Aluno
        }

        //public async Task<ApplicationUser> CreateAsync(ApplicationUser entity, string userName)
        //{
        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // Cria o ApplicationUser
        //        var result = await base.CreateAsync(entity);

        //        // Adiciona o Usuario correspondente
        //        var appUser = new Aluno(userName!, entity.Email!);

        //        if (Guid.TryParse(entity.Id, out var guid))
        //        {
        //            appUser.Id = guid;
        //        }
        //        else
        //        {
        //            throw new Exception("ID do usuário inválido.");
        //        }


        //        await _dbContext.Alunos.AddAsync(appUser);

        //        await _dbContext.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return result;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}
    }
}