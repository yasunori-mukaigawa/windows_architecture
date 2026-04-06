namespace IdIdentifyApp.Common.Infrastructure.Rest;

/**
 * 固定トークン提供クラス。
 *
 * 本クラスは、固定値の Bearer トークンを返却する簡易実装である。
 *
 * ■ 提供する責務
 *   固定アクセストークン返却
 *
 * ■ 設計上の意図
 *   初期実装や疎通確認時に、
 *   複雑な認証処理なしで API 呼び出しを成立させるための最小実装とする。
 *
 * ■ 注意
 *   本番環境では、必要に応じてログイン連携や
 *   トークン更新対応を持つ Provider 実装へ差し替えること。
 */
public sealed class FixedTokenProvider : ITokenProvider
{
    private readonly string? _token;

    public FixedTokenProvider()
    {
        _token = null;
    }

    public FixedTokenProvider(string? token)
    {
        _token = token;
    }

    /**
     * アクセストークンを返却する。
     */
    public Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_token);
    }
}