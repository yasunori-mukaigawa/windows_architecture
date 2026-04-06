namespace IdIdentifyApp.Ui.Common.Apps.Locators;

/**
 * ViewModel 解決契約。
 *
 * 本インターフェースは、View から必要な ViewModel を
 * 取得するための抽象契約を定義する。
 *
 * ■ 提供する責務
 *   指定型 ViewModel の解決
 *
 * ■ 設計上の意図
 *   ViewModelLocator が App や DI 実装詳細へ
 *   直接依存しないようにする。
 */
public interface IViewModelResolver
{
    /**
     * 指定した ViewModel を解決する。
     */
    T Resolve<T>() where T : class;
}