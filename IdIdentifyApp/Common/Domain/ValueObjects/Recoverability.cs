namespace IdIdentifyApp.Common.Domain.ValueObjects;

/**
 * 復旧属性値オブジェクト。
 *
 * ■ 提供する責務
 *   再試行可否
 *   スキップ可否
 *   再起動要否
 *
 * ■ 設計上の意図
 *   UI側の分岐ロジックを減らし、
 *   エラー自身が復旧可能性を持つようにする。
 */
public sealed record Recoverability(
    bool Retryable,
    bool Skippable,
    bool RestartRequired)
{
    /**
     * 復旧不可（デフォルト）
     */
    public static readonly Recoverability None =
        new(false, false, false);

    /**
     * 再試行可能
     */
    public static readonly Recoverability RetryableOnly =
        new(true, false, false);

    /**
     * 再起動必須
     */
    public static readonly Recoverability RestartRequiredOnly =
        new(false, false, true);
}