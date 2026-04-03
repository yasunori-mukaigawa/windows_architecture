using Microsoft.Extensions.DependencyInjection;

namespace IdIdentifyApp.Common.Apps.Hosting;

/**
 * モジュールサービス登録契約。
 *
 * 本インターフェースは、各モジュールが自身の依存関係を
 * DI コンテナへ登録するための契約を定義する。
 *
 * ■ 提供する責務
 *   モジュール固有サービス登録
 *
 * ■ 設計上の意図
 *   Common 層は Module の具象型を直接参照せず、
 *   契約ベースで各モジュールの登録処理を実行できるようにする。
 */
public interface IModuleServiceRegistrar
{
    /**
     * モジュール固有サービスを登録する。
     */
    void Register(IServiceCollection services);
}