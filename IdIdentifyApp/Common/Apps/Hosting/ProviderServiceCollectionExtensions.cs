using IdIdentifyApp;
using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Common.Apps.Hosting;

/**
 * Provider サービス登録拡張。
 *
 * 本クラスは、Provider 系の具象クラスを命名規約ベースで自動登録する。
 *
 * ■ 提供する責務
 *   Provider 自動登録
 *
 * ■ 設計上の意図
 *   TokenProvider などの共通提供系実装を
 *   命名規約で登録できるようにする。
 *
 * ■ 規約
 *   クラス名が "Provider" で終わる型を登録対象とする。
 */
public static class ProviderServiceCollectionExtensions
{
    /**
     * Provider を登録する。
     */
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<AppAssemblyMarker>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Provider")))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}