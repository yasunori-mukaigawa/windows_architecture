using System;
using IdIdentifyApp.Common.Domain.ValueObjects;

namespace IdIdentifyApp.Common.Domain.Errors;

/**
 * デバイス起因エラーを表す基底クラス。
 *
 * ■ 対象
 *   スキャナ
 *   ICリーダ
 *   プリンタ
 *   等
 *
 * ■ 設計上の意図
 *   外部ハードウェア依存のエラーを明確に分類し、
 *   再試行・再接続・再起動などの復旧判断に利用する。
 */
public abstract class DeviceError : DomainError
{
    protected DeviceError(
        ErrorCode code,
        string title,
        ErrorDetail detail,
        Recoverability? recoverability = null,
        Exception? cause = null)
        : base(
            code,
            ErrorCategory.Device,
            title,
            detail,
            recoverability ?? Recoverability.None,
            cause)
    {
    }
}