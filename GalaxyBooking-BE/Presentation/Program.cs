using BAL.Constants;
using BAL.Services.ZaloPay.Config;
using Microsoft.EntityFrameworkCore;
using Presentation.Extension;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.Configure<ZaloPayConfig>(builder.Configuration.GetSection(Constant.ZaloPayConfig.ConfigName));
builder.Services.ResolveServices(builder.Configuration);
builder.Services.ResolveDAL(connectionString);
builder.Services.ResolveController(jwtSettings, secretKey);
var app = builder.Build();
ConfigurePipeline(app);

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
    app.Run();
}