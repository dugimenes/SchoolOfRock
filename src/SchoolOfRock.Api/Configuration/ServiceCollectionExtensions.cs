using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolOfRock.Api.Services;
using SchoolOfRock.Domain.Models;
using System.Text;
using Aluno.Infra;
using Aluno.Infra.Repository;
using Conteudo.Infra;
using Conteudo.Infra.Repository;
using Identity.Domain.AggregateModel;
using Identity.Infra;
using Identity.Infra.Repository;
using Pagamento.Infra;
using Pagamento.Infra.Repository;
using SchoolOfRock.Shared.Repository;
using Identity.Infra.Services;

namespace SchoolOfRock.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultServices(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            services.AddEndpointsApiExplorer();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            if (environment == "Development")
            {
                var connection = configuration.GetConnectionString("SqliteConnection");
                services.AddDbContext<AlunoDbContext>(options =>
                    options.UseSqlite(connection,
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }
            else
            {
                var connection = configuration.GetConnectionString("SqlServerConnection");
                services.AddDbContext<AlunoDbContext>(options =>
                    options.UseSqlServer(connection,
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }

            if (environment == "Development")
            {
                var connection = configuration.GetConnectionString("SqliteConnection");
                services.AddDbContext<ConteudoDbContext>(options =>
                    options.UseSqlite(connection, 
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }
            else
            {
                var connection = configuration.GetConnectionString("SqlServerConnection");
                services.AddDbContext<ConteudoDbContext>(options =>
                    options.UseSqlServer(connection,
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }

            if (environment == "Development")
            {
                var connection = configuration.GetConnectionString("SqliteConnection");
                services.AddDbContext<PagamentoDbContext>(options =>
                    options.UseSqlite(connection,
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }
            else
            {
                var connection = configuration.GetConnectionString("SqlServerConnection");
                services.AddDbContext<PagamentoDbContext>(options =>
                    options.UseSqlServer(connection,
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }

            if (environment == "Development")
            {
                var connection = configuration.GetConnectionString("SqliteConnection");
                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlite(connection,
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }
            else
            {
                var connection = configuration.GetConnectionString("SqlServerConnection");
                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(connection, 
                        sql => sql.MigrationsAssembly("SchoolOfRock.Data")));
            }

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityDbContext>();

            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            
            if (string.IsNullOrEmpty(jwtSettings.Segredo))
            {
                throw new ArgumentNullException(nameof(jwtSettings.Segredo), "JWT Key is not defined in the configuration.");
            }

            services.AddHttpContextAccessor();

            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<IMatriculaRepository, MatriculaRepository>();

            if (!string.IsNullOrEmpty(jwtSettings?.Segredo))
            {
                var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo);

                services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = jwtSettings.Audiencia,
                            ValidIssuer = jwtSettings.Emissor
                        };
                    });
            }

            return services;
        }
    }
}