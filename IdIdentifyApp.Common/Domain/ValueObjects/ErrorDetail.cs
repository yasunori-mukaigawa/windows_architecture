namespace IdIdentifyApp.Common.Domain.ValueObjects;

/**
 * エラー詳細情報値オブジェクト。
 *
 * ■ 提供する責務
 *   ユーザー向けメッセージ
 *   サポート向け詳細メッセージ
 *
 * ■ 設計上の意図
 *   表示文言と内部情報を分離し、
 *   セキュリティと保守性を両立する。
 */
public sealed record ErrorDetail(
    string UserMessage,
    string? SupportMessage = null)
{
    public override string ToString()
        => UserMessage;
}