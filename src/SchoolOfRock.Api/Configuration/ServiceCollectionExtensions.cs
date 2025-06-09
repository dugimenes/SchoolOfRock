using Aluno.Application.Command;
using Aluno.Application.Validators;
using Aluno.Infra;
using Aluno.Infra.Repository;
using Conteudo.Application.Command;
using Conteudo.Application.Queries;
using Conteudo.Application.Validators;
using Conteudo.Infra;
using Conteudo.Infra.Repository;
using FluentValidation;
using Identity.Application.Command;
using Identity.Application.Queries;
using Identity.Domain.AggregateModel;
using Identity.Infra;
using Identity.Infra.Repository;
using Identity.Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pagamento.Infra;
using Pagamento.Infra.Repository;
using SchoolOfRock.Api.Behaviors;
using SchoolOfRock.Api.Services;
using SchoolOfRock.Domain.Models;
using System.Data.Common;
using System.Text;

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

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(RegisterCommand).Assembly,
                typeof(MatricularAlunoCommand).Assembly,
                typeof(LoginQuery).Assembly,
                typeof(CriarCursoCommand).Assembly,
                typeof(ObterCursoPorIdQuery).Assembly,
                typeof(Aluno.Application.Handlers.UsuarioCriadoHandler).Assembly,
                typeof(Pagamento.Application.Command.ConfirmarPagamentoCommand).Assembly
            ));

            services.AddValidatorsFromAssembly(typeof(RegisterCommand).Assembly);
            services.AddValidatorsFromAssembly(typeof(MatricularAlunoCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(CriarCursoCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

            services.AddScoped<DbConnection>(provider =>
            {
                var connectionString = environment == "Development"
                    ? configuration.GetConnectionString("SqliteConnection")
                    : configuration.GetConnectionString("SqlServerConnection");

                var connection = new SqliteConnection(connectionString);
                connection.Open();
                return connection;
            });

            Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions = (provider, options) =>
            {
                var connection = provider.GetRequiredService<DbConnection>();
                if (environment == "Development")
                {
                    options.UseSqlite(connection, sqlOptions =>
                        sqlOptions.MigrationsAssembly("SchoolOfRock.Data"));
                }
                else
                {
                    options.UseSqlServer(connection, sqlOptions =>
                        sqlOptions.MigrationsAssembly("SchoolOfRock.Data"));
                }
            };

            services.AddDbContext<AlunoDbContext>(dbContextOptions);
            services.AddDbContext<ConteudoDbContext>(dbContextOptions);
            services.AddDbContext<PagamentoDbContext>(dbContextOptions);
            services.AddDbContext<IdentityDbContext>(dbContextOptions);

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

            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            services.AddScoped<ICertificadoRepository, CertificadoRepository>();

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