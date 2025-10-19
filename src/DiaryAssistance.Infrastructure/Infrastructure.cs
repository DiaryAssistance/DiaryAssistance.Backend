using DiaryAssistance.Application.Options;
using DiaryAssistance.Application.Services;
using DiaryAssistance.Core.Consants;
using DiaryAssistance.Core.Entities;
using DiaryAssistance.Infrastructure.Services;
using DiaryAssistance.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiaryAssistance.Infrastructure;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        services.AddScoped<ITokenGenerator, TokenGenerator>();
        
        return services;
    }
    
    public static async Task Migrate(this IHost host)
    {
        await using var scope = host.Services.CreateAsyncScope();
        var dbContext =  scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    
        if (!await roleManager.RoleExistsAsync(Roles.Admin))
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin));
    
        if (!await roleManager.RoleExistsAsync(Roles.Teacher))
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Teacher));
    }
}