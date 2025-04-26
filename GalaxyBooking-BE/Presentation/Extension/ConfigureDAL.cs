using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Extension
{
    public static class ConfigureDAL
    {
        public static IServiceCollection ResolveDAL(this IServiceCollection services, string conn)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conn));
            return services;
        }
    }
}
