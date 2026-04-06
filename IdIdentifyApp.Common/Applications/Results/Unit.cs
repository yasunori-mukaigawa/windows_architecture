namespace IdIdentifyApp.Common.Applications.Results;

/**
 * 値を持たない成功結果を型として扱うための単位型。
 *
 * ■ 設計上の意図
 *   戻り値なしの成功も Result<Unit> として統一的に扱えるようにする。
 */
public readonly struct Unit
{
    public static readonly Unit Value = new();
}