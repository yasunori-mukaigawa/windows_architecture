using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Infrastructure.Rest;

/**
 * アクセストークン取得インターフェース。
 *
 * 本インターフェースは、API 呼び出し時に必要な認証トークンを取得する。
 *
 * ■ 提供する責務
 *   アクセストークン取得
 *
 * ■ 設計上の意図
 *   認証方式（固定トークン / OAuth / Refresh 等）を抽象化し、
 *   API 呼び出し層から分離する。
 */
public interface ITokenProvider
{
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken);
}