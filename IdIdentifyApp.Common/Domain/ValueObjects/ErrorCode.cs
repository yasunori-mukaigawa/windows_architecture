using System.Text.RegularExpressions;

namespace IdIdentifyApp.Common.Domain.ValueObjects;

/**
 * エラーコード値オブジェクト。
 *
 * ■ 形式
 *   CC-SSS-NNNN
 *   例：DEV-ICR-0001
 *
 * ■ 提供する責務
 *   フォーマット検証
 *   カテゴリ / サブシステム / 連番の分解
 *
 * ■ 設計上の意図
 *   文字列による表現のゆらぎを防ぎ、
 *   エラーコードの構造を型として扱う。
 */
public sealed record ErrorCode
{
    private static readonly Regex FormatRegex =
        new(@"^[A-Z]{3}-[A-Z]{3}-\d{4}$", RegexOptions.Compiled);

    public string Value { get; }

    public string Category { get; }

    public string Subsystem { get; }

    public string Number { get; }

    public ErrorCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("エラーコードは必須です。", nameof(value));
        }

        var normalized = value.Trim().ToUpperInvariant();

        if (!FormatRegex.IsMatch(normalized))
        {
            throw new ArgumentException(
                $"エラーコードの形式が不正です。value={value}, expected=CC-SSS-NNNN",
                nameof(value));
        }

        var parts = normalized.Split('-');
        Category = parts[0];
        Subsystem = parts[1];
        Number = parts[2];
        Value = normalized;
    }

    public static ErrorCode Create(string category, string subsystem, string number)
        => new($"{category}-{subsystem}-{number}");

    public override string ToString() => Value;
}