using System;

namespace IdIdentifyApp.Ui.Common.Apps.Locators;

/**
 * ViewModel 解決を一元化するための Locator。
 *
 * 本クラスは、Page から直接 App や DI コンテナへ
 * 依存させず、ViewModel の取得方法を 1 か所へ集約する。
 *
 * ■ 提供する責務
 *   ViewModelResolver を通じた ViewModel 解決
 *   View 側の依存解決コードの簡潔化
 *
 * ■ 設計上の意図
 *   Locator 自体は App を知らず、
 *   解決実体は外部から注入する構成とすることで
 *   依存方向を整理する。
 *
 * ■ 注意
 *   利用前に Initialize を 1 度だけ呼び出すこと。
 */
public static class ViewModelLocator
{
    private static IViewModelResolver? _resolver;

    /**
     * ViewModel 解決実体を初期化する。
     */
    public static void Initialize(IViewModelResolver resolver)
    {
        _resolver = resolver;
    }

    /**
     * 指定した ViewModel を解決する。
     *
     * 初期化前に呼ばれた場合は例外を送出する。
     */
    public static T Resolve<T>() where T : class
    {
        if (_resolver is null)
        {
            throw new InvalidOperationException(
                "ViewModelLocator is not initialized. Call Initialize() before Resolve().");
        }

        return _resolver.Resolve<T>();
    }
}