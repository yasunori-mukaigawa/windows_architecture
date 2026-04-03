using System;

namespace IdIdentifyApp.Modules.OperationLog.Domain.Entities;

/**
 * 操作ログを表す Entity。
 *
 * 本 Entity は、アプリケーション内で発生した操作イベントを
 * 永続化するための最小構成モデルを表す。
 *
 * ■ 保持する情報
 *   ログ識別子
 *   記録日時
 *   ログ種別
 *   イベントコード
 *   結果
 *   メッセージ
 *
 * ■ 設計上の意図
 *   まずは最小限の項目で保存できるようにし、
 *   後続で operation_log の要件に応じて列を拡張する。
 */
public sealed class OperationLogEntity
{
    /**
     * 操作ログ識別子。
     *
     * SQLite の主キーとして使用する。
     */
    public long OperationLogId { get; set; }

    /**
     * 記録日時。
     */
    public DateTimeOffset Timestamp { get; set; }

    /**
     * ログ種別。
     *
     * 例:
     *   UI
     *   PROCESS
     *   SESSION
     *   SETTING
     */
    public string LogType { get; set; } = string.Empty;

    /**
     * イベントコード。
     *
     * 例:
     *   SCAN_EXECUTE
     *   OCR_EXECUTE
     *   VERIFY_START
     */
    public string EventCode { get; set; } = string.Empty;

    /**
     * 実行結果。
     *
     * 例:
     *   START
     *   SUCCESS
     *   FAIL
     *   CANCEL
     */
    public string Result { get; set; } = string.Empty;

    /**
     * 補足メッセージ。
     *
     * エラー原因や詳細情報の記録に使用する。
     */
    public string? Message { get; set; }
}