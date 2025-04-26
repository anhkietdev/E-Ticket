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
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
// Add services to the container 
ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProjectionService, ProjectionService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IFilmGenreService, FilmGenreService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<TokenSettings>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//add zalopay config
builder.Services.Configure<ZaloPayConfig>(builder.Configuration.GetSection(Constant.ZaloPayConfig.ConfigName));
//add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Authentication to Swagger
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
builder.Services.AddAuthentication(options =>
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
    options.Audience = "your_audience";
    options.MapInboundClaims = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline
ConfigurePipeline(app);

app.UseAuthentication();

app.UseAuthorization();

app.Run();

// Configure Services
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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
    builder.Services.AddScoped<IGenreService, GenreService>();
    builder.Services.AddScoped<ITicketService, TicketService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<TokenSettings>();
    builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
    builder.Services.AddScoped<IEmailService, EmailService>();

    // ZaloPay configuration
    services.Configure<ZaloPayConfig>(configuration.GetSection(Constant.ZaloPayConfig.ConfigName));

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
}

// Configure Pipeline
void ConfigurePipeline(WebApplication app)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();

    app.UseAuthorization();
    // Global middleware
    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthorization();

    // Endpoint routing
    app.MapControllers();
}