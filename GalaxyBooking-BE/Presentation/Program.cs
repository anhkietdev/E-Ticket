using Microsoft.EntityFrameworkCore;
using Presentation.Extension;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container 
builder.Services.ResolveDAL(connectionString)
    .ResolveServices(builder.Configuration)
    .ResolveController(builder.Configuration)
    .AddJwtSwagger();
var app = builder.Build();

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

