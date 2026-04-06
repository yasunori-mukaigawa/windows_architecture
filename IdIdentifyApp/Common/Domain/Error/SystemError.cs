using System;
using IdIdentifyApp.Common.Domain.ValueObjects;

namespace IdIdentifyApp.Common.Domain.Errors;

/**
 * システム起因エラーを表す基底クラス。
 *
 * ■ 対象
 *   I/O失敗
 *   設定不整合
 *   想定外例外
 *
 * ■ 設計上の意図
 *   アプリケーションおよび基盤の異常を表現する。
 *   原則としてユーザー操作では解決できないケースが多い。
 */
public abstract class SystemError : DomainError
{
    protected SystemError(
        ErrorCode code,
        string title,
        ErrorDetail detail,
        Recoverability? recoverability = null,
        Exception? cause = null)
        : base(
            code,
            ErrorCategory.System,
            title,
            detail,
            recoverability ?? Recoverability.None,
            cause)
    {
    }
}