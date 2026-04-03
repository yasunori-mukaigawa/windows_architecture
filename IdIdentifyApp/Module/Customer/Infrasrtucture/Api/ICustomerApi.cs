using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Modules.Customer.Infrastructure.Api;

/**
 * 顧客 API 定義。
 *
 * 本インターフェースは、Refit による REST API 呼び出し定義を表現する。
 *
 * ■ 提供する責務
 *   HTTP エンドポイント定義
 *
 * ■ 設計上の意図
 *   Retrofit 風に宣言的に API を定義する。
 */
public interface ICustomerApi
{
    [Get("/customers/{customerId}")]
    Task<ApiResponse<CustomerResponseDto>> GetCustomerAsync(
        string customerId,
        CancellationToken cancellationToken = default);
}