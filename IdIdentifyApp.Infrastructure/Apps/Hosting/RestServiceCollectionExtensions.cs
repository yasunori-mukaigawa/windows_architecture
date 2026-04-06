using IdIdentifyApp.Common.Infrastructure.Rest;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace IdIdentifyApp.Infrastructure.Apps.Hosting;

/**
 * REST 共通基盤 DI 登録拡張。
 *
 * 本クラスは、REST API 利用に必要な
 * 共通コンポーネントを DI コンテナへ登録する。
 *
 * ■ 提供する責務
 *   認証ハンドラ登録
 *   TokenProvider 登録
 *   Refit API クライアント登録補助
 *
 * ■ 設計上の意図
 *   REST 呼び出し共通基盤を Common に集約し、
 *   Module 側は API 定義と業務変換に専念できるようにする。
 */
public static class RestServiceCollectionExtensions
{
    /**
     * REST 共通基盤を登録する。
     */
    public static IServiceCollection AddRestFoundation(this IServiceCollection services)
    {
        services.AddSingleton<ITokenProvider, FixedTokenProvider>();
        services.AddTransient<AuthHeaderHandler>();

        return services;
    }

    /**
     * Refit API クライアントを登録する。
     */
    public static IHttpClientBuilder AddRefitApiClient<TApi>(
        this IServiceCollection services,
        string baseUrl)
        where TApi : class
    {
        return services
            .AddRefitClient<TApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }
}