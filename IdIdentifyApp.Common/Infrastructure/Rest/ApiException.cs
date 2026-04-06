namespace IdIdentifyApp.Common.Infrastructure.Rest;

/**
 * API 呼び出し例外。
 *
 * 本クラスは、外部 API 呼び出し失敗を表現する。
 *
 * ■ 提供する責務
 *   HTTP エラーのラップ
 *
 * ■ 設計上の意図
 *   外部依存の例外を Application 層へ漏らさない。
 */
public sealed class ApiException : Exception
{
    public int StatusCode { get; }

    public ApiException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}