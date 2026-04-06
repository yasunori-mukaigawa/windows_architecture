using IdIdentifyApp.Common.Apps.Hosting;
using IdIdentifyApp.Infrastructure.Apps.Hosting;
using IdIdentifyApp.Infrastructure.Module.Customer.Api;
using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Module.Customer.Apps.Hosting;

/**
 * Customer モジュールサービス登録クラス。
 *
 * 本クラスは、Customer モジュール固有の依存関係を
 * DI コンテナへ登録する。
 *
 * ■ 提供する責務
 *   Customer API クライアント登録
 *
 * ■ 設計上の意図
 *   Customer モジュール固有の外部 API 依存を
 *   Module 側に閉じ込める。
 */
public sealed class CustomerModuleServiceRegistrar : IModuleServiceRegistrar
{
    /**
     * Customer モジュールサービスを登録する。
     */
    public void Register(IServiceCollection services)
    {
        services.AddRefitApiClient<ICustomerApi>("https://example.com");
    }
}