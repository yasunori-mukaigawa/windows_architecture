using IdIdentifyApp.Apps.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Common.Apps.Hosting;

/**
 * DI コンテナへのアプリケーションサービス登録をまとめる拡張クラス。
 *
 * 本クラスは、起動時に必要なサービスを ServiceCollection へ登録する責務を持つ。
 *
 * ■ 提供する責務
 *   Navigation 関連サービスの登録
 *   ShellWindow の登録
 *   ViewModel / UseCase / Port / Adapter の登録起点を提供する
 *
 * ■ 設計上の意図
 *   DI登録を App.xaml.cs に直書きせず、
 *   登録処理を専用クラスへ集約することで構成の見通しを良くする。
 */
public static class ServiceCollectionExtensions
{
    /**
     * アプリケーションで使用するサービスを DI コンテナへ登録する。
     */
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Navigation
        services.AddNavigation();

        // Repository
        services.AddRepositories();

        // UseCase
        services.AddUseCases();

        // ViewModel
        services.AddViewModels();

        // DB/Config
        services.AddCommonInfrastructure();

        // Refit
        services.AddRestFoundation();

        // Provider
        services.AddProviders();

        return services;
    }
}