using Microsoft.Extensions.DependencyInjection;


namespace IdIdentifyApp.Infrastructure.Apps.Hosting;

/**
 * 永続化 / 外部接続系サービス登録拡張。
 *
 * 本クラスは、Repository / Gateway 系具象クラスを
 * 命名規約ベースで自動登録する。
 *
 * ■ 提供する責務
 *   Repository 自動登録
 *   Gateway 自動登録
 *
 * ■ 設計上の意図
 *   Module 側の外部接続実装を明示列挙せず、
 *   命名規約に基づいて DI 登録を自動化する。
 *
 * ■ 規約
 *   クラス名が "Repository" または "Gateway" で終わる型を登録対象とする。
 */
public static class RepositoryServiceCollectionExtensions
{
    /**
     * Repository / Gateway 系サービスを登録する。
     */
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<AppAssemblyMarker>()
            .AddClasses(classes => classes.Where(type =>
                type.Name.EndsWith("Repository") ||
                type.Name.EndsWith("Gateway")))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}