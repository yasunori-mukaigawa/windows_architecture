using IdIdentifyApp.Common.Domain.Errors;
using IdIdentifyApp.Common.Domain.ValueObjects;
using System;

namespace IdIdentifyApp.Modules.Customer.Domain.Error;
public sealed class UnexpectedSystemError : SystemError
{
    public UnexpectedSystemError(Exception? cause = null)
        : base(
            new ErrorCode("SYS-UNK-9999"),
            "想定外エラー",
            new ErrorDetail(
                "システムエラーが発生しました。アプリを再起動してください。",
                cause?.Message),
            Recoverability.RestartRequiredOnly,
            cause)
    {
    }
}