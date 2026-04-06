using System;
using IdIdentifyApp.Common.Domain.ValueObjects;

namespace IdIdentifyApp.Common.Domain.Errors;

/**
 * 業務ルール不成立を表すエラー基底クラス。
 *
 * ■ 例
 *   券種不一致
 *   署名検証失敗
 *   データ不一致
 *
 * ■ 設計上の意図
 *   業務的に「成立しない」状態を表現する。
 *   システム異常ではなく、正常系の分岐として扱う。
 */
public abstract class BusinessError : DomainError
{
    protected BusinessError(
        ErrorCode code,
        string title,
        ErrorDetail detail,
        Recoverability? recoverability = null,
        Exception? cause = null)
        : base(
            code,
            ErrorCategory.Business,
            title,
            detail,
            recoverability ?? Recoverability.None,
            cause)
    {
    }
}