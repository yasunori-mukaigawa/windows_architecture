using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Apps.Hostings;

public static class ViewModelServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<AppAssemblyMarker>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("ViewModel")))
            .AsSelf()
            .WithTransientLifetime());


        return services;
    }
}