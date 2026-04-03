namespace IdIdentifyApp.Modules.Customer.Infrastructure.Api;

/**
 * 顧客 API レスポンス DTO。
 *
 * 本クラスは、外部 API のレスポンス構造を表現する。
 *
 * ■ 設計上の意図
 *   Domain モデルと分離することで、
 *   API 変更の影響範囲を局所化する。
 */
public sealed class CustomerResponseDto
{
    public required string CustomerId { get; init; }

    public required string Name { get; init; }

    public required string Status { get; init; }
}