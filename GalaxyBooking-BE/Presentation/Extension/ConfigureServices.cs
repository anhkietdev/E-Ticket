using BAL.DTOs;
using BAL.Services.Implement;
using BAL.Services.Interface;
using DAL.Context;
using DAL.Repository.Implement;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extension
{
    public static class ConfigureServices
    {
        public static IServiceCollection ResolveServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(30);
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

            // AutoMapper configuration
            services.AddAutoMapper(typeof(MappingProfile));

            // Register repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

            // Register services
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IZaloPayService, ZaloPayService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProjectionService, ProjectionService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<TokenSettings>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IEmailService, EmailService>();

            // CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Controllers and JSON configuration
            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    });

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
    }
}
