namespace IdIdentifyApp.Common.Constracts.Config;

/**
 * 設定読込の契約。
 *
 * 本契約は、指定された設定型を外部ソースから読み込む責務を持つ。
 *
 * ■ 提供する責務
 *   設定データの読込
 *   読込元の抽象化
 *
 * ■ 設計上の意図
 *   各機能がファイル読込や JSON デシリアライズ処理を直接持たず、
 *   設定取得手段を共通化する。
 *
 * ■ 注意
 *   設定値の妥当性判断や業務解釈は本契約の責務ではない。
 *   必要に応じて呼び出し側で検証する。
 */
public interface IConfigProvider<TConfig>
    where TConfig : class
{
    /**
     * 設定を同期的に読み込む。
     *
     * 起動時の初期化など、同期的に値を確定したい場面で使用する。
     */
    TConfig Load();

    /**
     * 設定を非同期で読み込む。
     */
    Task<TConfig> LoadAsync(CancellationToken cancellationToken = default);
}