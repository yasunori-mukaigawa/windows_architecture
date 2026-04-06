using IdIdentifyApp.Common.Domain.ValueObjects;

namespace IdIdentifyApp.Common.Domain.Error;

/**
 * アプリケーション全体で扱う共通エラー基底クラス。
 *
 * 本クラスは、UseCase および UI 層で扱うエラー表現を統一する責務を持つ。
 * Infrastructure 層で発生した例外は、本クラスまたは派生クラスへ変換される。
 *
 * ■ 提供する責務
 *   エラーコードの保持
 *   エラーカテゴリの保持
 *   ユーザー向け / サポート向けメッセージの保持
 *   復旧可否（再試行・スキップ・再起動）の保持
 *   内部例外の保持（ログ・デバッグ用途）
 *
 * ■ 設計上の意図
 *   例外（Exception）と業務エラーを分離し、
 *   UI 層が例外詳細を直接扱わないようにする。
 *   また、エラーを ValueObject で構成することで、
 *   文字列や bool の意味の混在を防ぐ。
 */
public abstract class DomainError
{
    protected DomainError(
        ErrorCode code,
        ErrorCategory category,
        string title,
        ErrorDetail detail,
        Recoverability recoverability,
        Exception? cause = null)
    {
        Code = code;
        Category = category;
        Title = title;
        Detail = detail;
        Recoverability = recoverability;
        Cause = cause;
    }

    /**
     * エラーコード。
     *
     * 例：DEV-ICR-0001
     */
    public ErrorCode Code { get; }

    /**
     * エラーカテゴリ。
     *
     * 例：Business / Validation / Device / System
     */
    public ErrorCategory Category { get; }

    /**
     * エラータイトル。
     *
     * ダイアログタイトルや画面見出しに利用する。
     */
    public string Title { get; }

    /**
     * エラー詳細情報。
     *
     * ユーザー向けメッセージとサポート向け詳細を保持する。
     */
    public ErrorDetail Detail { get; }

    /**
     * 復旧属性。
     *
     * 再試行可否・スキップ可否・再起動要否を保持する。
     */
    public Recoverability Recoverability { get; }

    /**
     * ユーザー向けメッセージ。
     */
    public string UserMessage => Detail.UserMessage;

    /**
     * サポート向け詳細メッセージ。
     */
    public string? SupportMessage => Detail.SupportMessage;

    /**
     * 再試行可能かどうか。
     */
    public bool Retryable => Recoverability.Retryable;

    /**
     * スキップ可能かどうか。
     */
    public bool Skippable => Recoverability.Skippable;

    /**
     * 再起動が必要かどうか。
     */
    public bool RestartRequired => Recoverability.RestartRequired;

    /**
     * 内部例外（原因例外）。
     *
     * ログ出力やデバッグ用途で利用する。
     */
    public Exception? Cause { get; }

    public override string ToString()
        => $"{Code} [{Category}] : {Title}";
}