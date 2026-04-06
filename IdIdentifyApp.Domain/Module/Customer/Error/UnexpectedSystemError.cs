using IdIdentifyApp.Common.Domain.Error;
using IdIdentifyApp.Common.Domain.ValueObjects;

namespace IdIdentifyApp.Domain.Module.Customer.Error;
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