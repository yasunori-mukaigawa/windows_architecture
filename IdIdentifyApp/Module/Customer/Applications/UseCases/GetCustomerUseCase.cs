using IdIdentifyApp.Modules.Customer.Applications.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Modules.Customer.Applications.UseCases;

/**
 * 顧客取得 UseCase。
 *
 * 本 UseCase は、顧客 ID をもとに顧客情報を取得する
 * 業務処理を表現する。
 *
 * ■ 提供する責務
 *   顧客取得要求実行
 *
 * ■ 設計上の意図
 *   UI から見た目的単位の処理として、
 *   外部 API 利用を業務操作単位へまとめる。
 */
public sealed class GetCustomerUseCase
{
    private readonly ICustomerApiGateway _customerGateway;

    public GetCustomerUseCase(ICustomerApiGateway customerGateway)
    {
        _customerGateway = customerGateway;
    }

    /**
     * 顧客取得処理を実行する。
     */
    public Task<CustomerResult> ExecuteAsync(
        string customerId,
        CancellationToken cancellationToken)
    {
        return _customerGateway.GetCustomerAsync(customerId, cancellationToken);
    }
}