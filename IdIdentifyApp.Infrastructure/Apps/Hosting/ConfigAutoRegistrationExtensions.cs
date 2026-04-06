using IdIdentifyApp.Common.Constracts.Config;
using IdIdentifyApp.Common.Infrastructure.Config;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdIdentifyApp.Infrastructure.Apps.Hosting;

/**
 * Config Provider の規約ベース自動登録を行う拡張クラス。
 *
 * 本クラスは、IAppConfig を実装した型を Assembly から検出し、
 * 型名に基づく JSON ファイル名で IConfigProvider<T> を自動登録する。
 *
 * ■ ファイル名規約
 *   AppSettings -> appsettings.json
 *   ApiClientSettings -> apiclientsettings.json
 */
public static class ConfigAutoRegistrationExtensions
{
    /**
     * 指定 Assembly から設定型を検出し、自動登録する。
     *
     * excludePredicate を指定した場合は、
     * true を返した型を登録対象から除外する。
     */
    public static IServiceCollection AddJsonConfigProvidersFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        string? baseDirectory = null,
        Func<Type, bool>? excludePredicate = null)
    {
        baseDirectory ??= AppContext.BaseDirectory;
        excludePredicate ??= _ => false;

        var configDirectory = Path.Combine(baseDirectory, "config");
        Directory.CreateDirectory(configDirectory);

        var configTypes = assembly
            .GetTypes()
            .Where(t =>
                t is { IsClass: true, IsAbstract: false } &&
                typeof(IAppConfig).IsAssignableFrom(t) &&
                !excludePredicate(t));

        foreach (var configType in configTypes)
        {
            RegisterConfigProvider(services, configType, configDirectory);
        }

        return services;
    }

    /**
     * 指定設定型の Config Provider を登録する。
     */
    private static void RegisterConfigProvider(
        IServiceCollection services,
        Type configType,
        string configDirectory)
    {
        var fileName = ToJsonFileName(configType.Name);
        var filePath = Path.Combine(configDirectory, fileName);

        var method = typeof(ConfigAutoRegistrationExtensions)
            .GetMethod(nameof(RegisterConfigProviderGeneric), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(configType);

        method.Invoke(null, new object[] { services, filePath });
    }

    /**
     * Config Provider を登録するジェネリック実装。
     */
    private static void RegisterConfigProviderGeneric<TConfig>(
        IServiceCollection services,
        string filePath)
        where TConfig : class, IAppConfig
    {
        services.AddSingleton<IConfigProvider<TConfig>>(_ =>
            new JsonConfigProvider<TConfig>(filePath));
    }

    /**
     * 設定型から JSON ファイル名を生成する。
     */
    private static string ToJsonFileName(string typeName)
    {
        return $"{typeName.ToLowerInvariant()}.json";
    }
}