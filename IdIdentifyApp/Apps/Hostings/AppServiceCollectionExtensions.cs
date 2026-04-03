using System.Reflection;
using IdIdentifyApp.Common.Apps.Hosting;
using IdIdentifyApp.Modules.Customer;
using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Apps.Hosting;

/**
 * アプリケーション全体サービス登録拡張。
 *
 * 本クラスは、共通基盤サービス登録と
 * モジュールサービス登録を組み合わせて、
 * アプリケーション全体の DI 構成を組み立てる。
 *
 * ■ 提供する責務
 *   共通基盤登録
 *   モジュール登録実行
 *
 * ■ 設計上の意図
 *   Common と Module の依存関係を分離しつつ、
 *   起動時のサービス登録を 1 箇所へ集約する。
 */
public static class AppServiceCollectionExtensions
{
    /**
     * アプリケーション全体サービスを登録する。
     */
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddApplicationServices();

        services.AddModuleServices(
            typeof(AppAssemblyMarker).Assembly);

        return services;
    }
}