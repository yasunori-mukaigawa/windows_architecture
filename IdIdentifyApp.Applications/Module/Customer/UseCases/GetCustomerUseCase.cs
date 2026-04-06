using IdIdentifyApp.Applications.Module.Customer.Ports;
using IdIdentifyApp.Common.Applications.Results;
using IdIdentifyApp.Domain.Module.Customer.Error;

namespace IdIdentifyApp.Applications.Module.Customer.UseCases;

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
    public async Task<Result<CustomerResult>> ExecuteAsync(
        string customerId,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _customerGateway.GetCustomerAsync(customerId, cancellationToken);

            return Result<CustomerResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<CustomerResult>.Failure(
                new UnexpectedSystemError(ex));
        }
    }
}
