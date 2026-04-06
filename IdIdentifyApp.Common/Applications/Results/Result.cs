// ==========================================================
// File: Common/Application/Results/Result.cs
// ==========================================================
using IdIdentifyApp.Common.Domain.Error;

namespace IdIdentifyApp.Common.Applications.Results;

/**
 * UseCase の実行結果を表す共通結果型。
 *
 * 本クラスは、成功または失敗のいずれかを明示的に表現する責務を持つ。
 * 失敗時は DomainError を保持し、ViewModel はそれを Message / State / Effect
 * へ変換して UI 表示へ反映する。
 *
 * ■ 提供する責務
 *   成功状態 / 失敗状態の表現
 *   成功時の値保持
 *   失敗時の DomainError 保持
 *
 * ■ 設計上の意図
 *   UseCase の戻り値を型として統一し、
 *   例外による制御フローを減らす。
 */
public class Result
{
    protected Result(bool isSuccess, DomainError? error)
    {
        if (isSuccess && error is not null)
        {
            throw new ArgumentException("成功結果に Error は設定できません。", nameof(error));
        }

        if (!isSuccess && error is null)
        {
            throw new ArgumentException("失敗結果には Error が必須です。", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /**
     * 成功結果かどうか。
     */
    public bool IsSuccess { get; }

    /**
     * 失敗結果かどうか。
     */
    public bool IsFailure => !IsSuccess;

    /**
     * 失敗時の DomainError。
     *
     * 成功時は null。
     */
    public DomainError? Error { get; }

    /**
     * 成功結果を生成する。
     */
    public static Result Success()
    {
        return new Result(true, null);
    }

    /**
     * 失敗結果を生成する。
     */
    public static Result Failure(DomainError error)
    {
        return new Result(false, error);
    }
}


/**
 * 値を伴う UseCase の実行結果を表す共通結果型。
 *
 * 本クラスは、成功時に結果値を保持し、
 * 失敗時に DomainError を保持する責務を持つ。
 *
 * ■ 提供する責務
 *   成功状態 / 失敗状態の表現
 *   成功時の値保持
 *   失敗時の DomainError 保持
 *
 * ■ 設計上の意図
 *   UseCase の戻り値と失敗情報を 1 つの型へ集約し、
 *   成功 / 失敗の分岐を明確にする。
 */
public sealed class Result<T> : Result
{
    private Result(T value)
        : base(true, null)
    {
        Value = value;
    }

    private Result(DomainError error)
        : base(false, error)
    {
        Value = default;
    }

    /**
     * 成功時の結果値。
     *
     * 失敗時は default。
     */
    public T? Value { get; }

    /**
     * 成功結果を生成する。
     */
    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    /**
     * 失敗結果を生成する。
     */
    public static new Result<T> Failure(DomainError error)
    {
        return new Result<T>(error);
    }

    /**
     * 成功時の値を取得する。
     *
     * 失敗時は例外を送出する。
     */
    public T GetValueOrThrow()
    {
        if (IsFailure)
        {
            throw new InvalidOperationException("失敗結果のため Value は取得できません。");
        }

        return Value!;
    }
}