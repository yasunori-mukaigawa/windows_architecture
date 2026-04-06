using System;
using IdIdentifyApp.Common.Domain.ValueObjects;

namespace IdIdentifyApp.Common.Domain.Errors;

/**
 * 入力不備・検証エラーを表す基底クラス。
 *
 * ■ 例
 *   必須未入力
 *   形式不正
 *   桁数不正
 *
 * ■ 設計上の意図
 *   ユーザー操作によって修正可能なエラーを表現する。
 *   原則として再試行可能なエラーとして扱う。
 */
public abstract class ValidationError : DomainError
{
    protected ValidationError(
        ErrorCode code,
        string title,
        ErrorDetail detail,
        Recoverability? recoverability = null,
        Exception? cause = null)
        : base(
            code,
            ErrorCategory.Validation,
            title,
            detail,
            recoverability ?? Recoverability.RetryableOnly,
            cause)
    {
    }
}