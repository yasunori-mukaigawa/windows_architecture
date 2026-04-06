using IdIdentifyApp.Ui.Common.Apps.Locators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdIdentifyApp.Apps.Locators;

/**
 * IServiceProvider を用いた ViewModelResolver 実装。
 *
 * 本クラスは、DI コンテナから ViewModel を解決する
 * アプリ側実装を提供する。
 *
 * ■ 提供する責務
 *   IServiceProvider を通じた ViewModel 解決
 *
 * ■ 設計上の意図
 *   Ui.Common 側へ App 依存を持ち込まず、
 *   実行アプリ側で DI 実装を差し込む。
 */
public sealed class ServiceProviderViewModelResolver : IViewModelResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderViewModelResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /**
     * 指定した ViewModel を DI コンテナから取得する。
     */
    public T Resolve<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}