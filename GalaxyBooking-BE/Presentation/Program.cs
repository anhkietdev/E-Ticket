using BAL.Constants;
using BAL.DTOs;
using BAL.Services.Implement;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Config;
using DAL.Context;
using DAL.Repository.Implement;
using DAL.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

// Add services to the container 
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigurePipeline(app);

// Configure Services
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Database configuration
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

    services.AddHttpContextAccessor();

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

   
    services.Configure<ZaloPayConfig>(configuration.GetSection(Constant.ZaloPayConfig.ConfigName));

    
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter 'Bearer {your JWT token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
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
                new List<string>()
            }
        });
    });

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ClockSkew = TimeSpan.Zero
        };
        options.Audience = jwtSettings["Audience"];
        options.MapInboundClaims = false; 
    });
}


void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthentication(); 

    app.UseAuthorization();

    app.MapControllers();
    app.Run();
}
