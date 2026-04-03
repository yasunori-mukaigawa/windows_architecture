using IdIdentifyApp.Common.Apps.Navigation;
using IdIdentifyApp.Common.Apps.Shell;
using Microsoft.Extensions.DependencyInjection;

public static class NavigationServiceCollectionExtensions
{
    public static IServiceCollection AddNavigation(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ShellWindow>();

        return services;
    }
}