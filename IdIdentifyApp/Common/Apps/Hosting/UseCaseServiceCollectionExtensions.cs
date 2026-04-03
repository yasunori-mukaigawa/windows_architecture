using IdIdentifyApp;
using IdIdentifyApp.Feature.Check.Ui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

public static class UseCaseServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<AppAssemblyMarker>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("UseCase")))
            .AsSelf()
            .WithTransientLifetime());

        services.Scan(scan => scan
            .FromAssemblyOf<AppAssemblyMarker>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("UseCases")))
            .AsSelf()
            .WithTransientLifetime());

        return services;
    }
}