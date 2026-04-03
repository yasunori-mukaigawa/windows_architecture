using IdIdentifyApp.Common.Infrastructure.Rest;
using IdIdentifyApp.Modules.Customer.Applications.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Modules.Customer.Infrastructure.Api;

/**
 * 顧客 API Gateway 実装。
 *
 * 本クラスは、Refit API を利用して顧客情報を取得する。
 *
 * ■ 提供する責務
 *   API 呼び出し
 *   DTO → Domain 変換
 *
 * ■ 設計上の意図
 *   外部 API 依存を Infrastructure に閉じ込める。
 */
public sealed class CustomerApiGateway : ICustomerApiGateway
{
    private readonly ICustomerApi _api;

    public CustomerApiGateway(ICustomerApi api)
    {
        _api = api;
    }

    public async Task<CustomerResult> GetCustomerAsync(
        string customerId,
        CancellationToken cancellationToken)
    {
        var response = await _api.GetCustomerAsync(customerId, cancellationToken);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            throw new ApiException(
                "Customer API call failed",
                (int)response.StatusCode);
        }

        var dto = response.Content;

        return new CustomerResult(
            dto.CustomerId,
            dto.Name,
            dto.Status);
    }
}