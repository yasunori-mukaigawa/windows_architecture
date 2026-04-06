using IdIdentifyApp.Common.Domain.Error;

namespace IdIdentifyApp.Common.Applications.Results;

/**
 * Result 用の拡張メソッド群。
 *
 * 本クラスは、Result の成功 / 失敗ハンドリングを簡潔に記述する責務を持つ。
 */
public static class ResultExtensions
{
    /**
     * 成功時に指定処理を実行する。
     */
    public static Result OnSuccess(this Result result, Action action)
    {
        if (result.IsSuccess)
        {
            action();
        }

        return result;
    }

    /**
     * 失敗時に指定処理を実行する。
     */
    public static Result OnFailure(this Result result, Action<DomainError> action)
    {
        if (result.IsFailure)
        {
            action(result.Error!);
        }

        return result;
    }

    /**
     * 成功時に指定処理を実行する。
     */
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value!);
        }

        return result;
    }

    /**
     * 失敗時に指定処理を実行する。
     */
    public static Result<T> OnFailure<T>(this Result<T> result, Action<DomainError> action)
    {
        if (result.IsFailure)
        {
            action(result.Error!);
        }

        return result;
    }

    /**
     * 成功値を別の型へ変換する。
     *
     * 失敗時は Error をそのまま引き継ぐ。
     */
    public static Result<TResult> Map<T, TResult>(
        this Result<T> result,
        Func<T, TResult> mapper)
    {
        if (result.IsFailure)
        {
            return Result<TResult>.Failure(result.Error!);
        }

        return Result<TResult>.Success(mapper(result.Value!));
    }

    /**
     * 成功値を Result<TResult> へ変換する。
     *
     * 失敗時は Error をそのまま引き継ぐ。
     */
    public static Result<TResult> Bind<T, TResult>(
        this Result<T> result,
        Func<T, Result<TResult>> binder)
    {
        if (result.IsFailure)
        {
            return Result<TResult>.Failure(result.Error!);
        }

        return binder(result.Value!);
    }
}