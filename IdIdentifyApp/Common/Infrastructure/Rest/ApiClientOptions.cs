namespace IdIdentifyApp.Common.Infrastructure.Rest;

/**
 * API クライアント設定。
 *
 * 本クラスは、外部 REST API 呼び出しにおける共通設定を保持する。
 *
 * ■ 提供する責務
 *   BaseUrl の管理
 *   タイムアウト設定の定義
 *
 * ■ 設計上の意図
 *   API ごとの設定を DI で注入可能にし、
 *   環境ごとの切り替えを容易にする。
 */
public sealed class ApiClientOptions
{
    public required string BaseUrl { get; init; }

    public int TimeoutSeconds { get; init; } = 30;
}