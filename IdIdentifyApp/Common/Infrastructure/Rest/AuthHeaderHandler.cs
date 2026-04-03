using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Infrastructure.Rest;

/**
 * 認証ヘッダ付与ハンドラ。
 *
 * 本クラスは、HTTP リクエストに対して Authorization ヘッダを付与する。
 *
 * ■ 提供する責務
 *   Bearer トークンの自動付与
 *
 * ■ 設計上の意図
 *   API 呼び出しコードから認証処理を分離し、
 *   横断的関心事として共通化する。
 */
public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider;

    public AuthHeaderHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}