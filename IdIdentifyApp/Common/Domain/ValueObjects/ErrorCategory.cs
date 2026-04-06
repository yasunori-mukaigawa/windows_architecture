namespace IdIdentifyApp.Common.Domain.ValueObjects;

/**
 * エラーカテゴリ値オブジェクト。
 *
 * ■ 種別
 *   Business / Validation / Device / System
 *
 * ■ 設計上の意図
 *   エラーの分類を文字列ではなく型として扱い、
 *   分岐やログ集計を安定させる。
 */
public sealed record ErrorCategory(string Value)
{
    public static readonly ErrorCategory Business = new("Business");
    public static readonly ErrorCategory Validation = new("Validation");
    public static readonly ErrorCategory Device = new("Device");
    public static readonly ErrorCategory System = new("System");

    public override string ToString() => Value;
}