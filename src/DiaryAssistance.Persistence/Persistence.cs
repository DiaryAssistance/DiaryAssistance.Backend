using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiaryAssistance.Persistence;

public static class Persistence
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found");
        services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(connectionString));
        return services;
    }
}