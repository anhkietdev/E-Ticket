using BAL.Constants;
using BAL.DTOs;
using BAL.Services.Implement;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Config;
using DAL.Context;
using DAL.Repository.Implement;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container 
ConfigureServices(builder.Services, builder.Configuration);

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProjectionService, ProjectionService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ITicketService, TicketService>();
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


var app = builder.Build();

// Configure the HTTP request pipeline
ConfigurePipeline(app);

app.Run();

// Configure Services
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Database configuration
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString, sqlOptions => {
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

    // Global middleware
    app.UseCors("AllowAll");
    app.UseHttpsRedirection();
    app.UseAuthorization();

    // Endpoint routing
    app.MapControllers();
}