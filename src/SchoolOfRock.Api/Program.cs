using Aluno.Infra;
using Conteudo.Infra;
using Conteudo.Infra.Seeders;
using Identity.Infra;
using Identity.Infra.Seeders;
using Microsoft.EntityFrameworkCore;
using Pagamento.Infra;
using SchoolOfRock.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

builder.Services.AddDefaultServices(builder.Configuration, environment);

var app = builder.Build();

async Task InitializeDatabaseAsync(IApplicationBuilder webApp)
{
    using (var scope = webApp.ApplicationServices.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;

        try
        {
            var alunoContext = serviceProvider.GetRequiredService<AlunoDbContext>();
            await alunoContext.Database.MigrateAsync();

            var conteudoContext = serviceProvider.GetRequiredService<ConteudoDbContext>();
            await conteudoContext.Database.MigrateAsync();

            var pagamentoContext = serviceProvider.GetRequiredService<PagamentoDbContext>();
            await pagamentoContext.Database.MigrateAsync();

            var identityContext = serviceProvider.GetRequiredService<IdentityDbContext>();
            await identityContext.Database.MigrateAsync();

            await IdentityDataSeeder.SeedRolesAsync(serviceProvider);
            await CursoSeeder.SeedAsync(conteudoContext);
            await AulaSeeder.SeedAsync(conteudoContext);
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ocorreu um erro durante a inicialização do banco de dados.");
            throw;
        }
    }
}

await InitializeDatabaseAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSession();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();