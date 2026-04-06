using IdIdentifyApp.Apps.Shell;
using IdIdentifyApp.Ui.Common.Apps.Navigation;
using IdIdentifyApp.Ui.Common.Apps.Shell;
using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Apps.Hostings;

/**
 * Navigation / Shell 関連サービスの DI 登録拡張。
 *
 * 本クラスは、画面遷移基盤および
 * Shell 実行に必要なサービスを DI コンテナへ登録する。
 *
 * ■ 提供する責務
 *   NavigationService の登録
 *   ShellEffectCoordinator の登録
 *   ShellWindow の登録
 *
 * ■ 設計上の意図
 *   UI 共通基盤と実アプリ Shell を接続する
 *   登録処理を 1 箇所へ集約する。
 */
public static class NavigationServiceCollectionExtensions
{
    /**
     * Navigation / Shell 関連サービスを登録する。
     */
    public static IServiceCollection AddNavigation(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ShellEffectCoordinator>();
        services.AddSingleton<ShellWindow>();

        return services;
    }
}