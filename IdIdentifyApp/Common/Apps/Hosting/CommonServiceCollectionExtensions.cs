using IdIdentifyApp.Common.Contracts.Config;
using IdIdentifyApp.Common.Infrastructure.Config;
using IdIdentifyApp.Common.Infrastructure.Db;
using IdIdentifyApp.Common.Settings;
using IdIdentifyApp.Common.Settings.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace IdIdentifyApp.Apps.Hosting;

/**
 * 共通基盤サービスを登録する拡張クラス。
 *
 * 本クラスは、Config 基盤および DB 基盤を
 * DI コンテナへ登録する責務を持つ。
 *
 * ■ 提供する責務
 *   AppSettings 用 Config Provider の登録
 *   その他 Config Provider の自動登録
 *   DbContextFactory の自動登録
 *   DB 初期化サービスの登録
 *
 * ■ 設計上の意図
 *   Common 層の基盤サービスを 1 箇所へ集約し、
 *   起動時の依存関係構築を明確化する。
 *
 * ■ 注意
 *   本メソッドでは AppSettings を同期的に読み込み、
 *   DB 接続やログ出力先に必要な情報を確定させる。
 */
public static class CommonServiceCollectionExtensions
{
    /**
     * 共通基盤サービスを登録する。
     */
    public static IServiceCollection AddCommonInfrastructure(this IServiceCollection services)
    {
        var assembly = typeof(AppAssemblyMarker).Assembly;
        var baseDirectory = AppContext.BaseDirectory;

        // ============================
        // AppSettings Provider 構築
        // ============================

        var appSettingsProvider = CreateAppSettingsProvider(baseDirectory);

        // AppSettings は起動時に即利用するため先に登録する
        services.AddSingleton<IConfigProvider<AppSettings>>(_ => appSettingsProvider);

        // ============================
        // AppSettings 読込
        // ============================

        var settings = appSettingsProvider.Load();

        // 基盤用ディレクトリを初期化する
        EnsureDirectory(settings.DataDirectory);
        EnsureDirectory(settings.LogDirectory);

        // ============================
        // その他 Config 自動登録
        // ============================

        services.AddJsonConfigProvidersFromAssembly(
            assembly,
            baseDirectory,
            excludePredicate: type => type == typeof(AppSettings));

        // ============================
        // DbContext 自動登録
        // ============================

        services.AddSqliteDbContextsFromAssembly(assembly, settings.DataDirectory);

        // ============================
        // DB 初期化サービス
        // ============================

        services.AddSingleton<AppDbContextInitializer>();

        return services;
    }

    /**
     * AppSettings 用 ConfigProvider を生成する。
     */
    private static JsonConfigProvider<AppSettings> CreateAppSettingsProvider(string baseDirectory)
    {
        var configPath = Path.Combine(baseDirectory, "config", "appsettings.json");

        return new JsonConfigProvider<AppSettings>(configPath);
    }

    /**
     * 指定ディレクトリを初期化する。
     *
     * 存在しない場合は作成する。
     */
    private static void EnsureDirectory(string directoryPath)
    {
        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}