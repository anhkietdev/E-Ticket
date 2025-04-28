using BAL.Services.Interface;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Presentation.Extension;


var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container 
builder.Services.ResolveDAL(connectionString)
    .ResolveServices(builder.Configuration)
    .ResolveController(builder.Configuration)
.AddJwtSwagger();

builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));

builder.Services.AddHangfireServer();

var app = builder.Build();

var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<IFilmService>(
    recurringJobId: "update-film-status-job",
    methodCall: service => service.UpdateFilmStatusCronJobs(),
    cronExpression: "0 23 * * *"
);

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();

