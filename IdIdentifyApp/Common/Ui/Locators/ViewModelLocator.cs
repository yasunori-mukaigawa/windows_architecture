using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdIdentifyApp.Common.Ui.Locators;

/**
 * ViewModel 解決を一元化するための Locator。
 *
 * 本クラスは、Page から直接 App.Services を参照させず、
 * ViewModel の取得方法を 1 か所へ集約することを目的とする。
 *
 * ■ 提供する責務
 *   DI コンテナから ViewModel を取得する
 *   Page 側の依存解決コードを簡潔にする
 *
 * ■ 設計上の意図
 *   現時点では Service Locator 的な実装だが、
 *   将来的に PageFactory 等へ移行する際の中継点として機能する。
 */
public static class ViewModelLocator
{
    /**
     * 指定した ViewModel を DI コンテナから取得する。
     *
     * 対象型が未登録の場合は例外を送出する。
     */
    public static T Resolve<T>() where T : class
    {
        return App.Services.GetRequiredService<T>();
    }
}