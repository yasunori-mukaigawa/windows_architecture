using IdIdentifyApp.Common.Contracts.Config;
using System;
using System.IO;

namespace IdIdentifyApp.Common.Settings.Domain;

/**
 * アプリケーション共通の基盤設定。
 *
 * 本設定は、DB やログ出力先など、
 * Common 層の初期化に必要な設定値を表す。
 *
 * ■ 設計上の意図
 *   Common 層は Module / Feature に依存しないようにし、
 *   起動時に必要な設定値は Common 側で完結させる。
 *
 * ■ 規約
 *   本型は規約ベース自動登録対象であるため、
 *   public parameterless constructor を持てる形で定義する。
 */
public sealed record AppSettings : IAppConfig
{
    /**
     * DB ファイル格納先ディレクトリ。
     */
    public string DataDirectory { get; init; } =
        Path.Combine(AppContext.BaseDirectory, "data");

    /**
     * ログファイル格納先ディレクトリ。
     */
    public string LogDirectory { get; init; } =
        Path.Combine(AppContext.BaseDirectory, "logs");
}