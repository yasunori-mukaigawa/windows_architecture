namespace IdIdentifyApp.Common.Contracts.Config;

/**
 * JSON 設定型であることを表すマーカー契約。
 *
 * 本契約を実装した型は、
 * 規約ベースの Config Provider 自動登録対象となる。
 *
 * ■ 設計上の意図
 *   Assembly スキャン時に、どの型を設定型として扱うかを
 *   明示的に判定できるようにする。
 */
public interface IAppConfig
{
}