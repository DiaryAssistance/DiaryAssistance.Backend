using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiaryAssistance.Application;

public static class Application
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(Application).Assembly);
        services.AddMediatR(opts =>
        {
            opts.RegisterServicesFromAssembly(typeof(Application).Assembly);
            opts.LicenseKey = configuration["MediatrLicenseKey"];
        });
        return services;
    }
}