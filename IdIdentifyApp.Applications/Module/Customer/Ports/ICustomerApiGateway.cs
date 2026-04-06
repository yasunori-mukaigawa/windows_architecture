namespace IdIdentifyApp.Applications.Module.Customer.Ports;

/**
 * 顧客 API Gateway インターフェース。
 *
 * 本インターフェースは、外部 API を通じた顧客取得処理を抽象化する。
 *
 * ■ 提供する責務
 *   顧客情報取得
 *
 * ■ 設計上の意図
 *   UseCase を外部 API 実装から分離する。
 */
public interface ICustomerApiGateway
{
    Task<CustomerResult> GetCustomerAsync(
        string customerId,
        CancellationToken cancellationToken);
}

/**
 * 顧客ドメイン結果。
 */
public sealed record CustomerResult(
    string CustomerId,
    string Name,
    string Status);