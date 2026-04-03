using IdIdentifyApp.Apps.Hosting;
using IdIdentifyApp.Common.Apps.Hosting;
using IdIdentifyApp.Common.Apps.Shell;
using IdIdentifyApp.Common.Infrastructure.Db;
using IdIdentifyApp.Modules.OperationLog.Application.Ports;
using IdIdentifyApp.Modules.OperationLog.Domain.Entities;
using IdIdentifyApp.Modules.OperationLog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Serilog;
using System;

namespace IdIdentifyApp;

/**
 * アプリケーション全体の起動エントリポイント。
 *
 * 本クラスは、WinUI3 アプリケーションの起動時に必要となる
 * ホスト生成、DI 初期化、メインウィンドウ生成を担当する。
 *
 * ■ 提供する責務
 *   Generic Host の生成
 *   DI コンテナの初期化
 *   ログ基盤の初期化
 *   ShellWindow の生成と起動
 *
 * ■ 設計上の意図
 *   アプリケーションの起動処理と依存解決の責務を本クラスへ集約し、
 *   View や ViewModel 側へ初期化責務を持ち込まない。
 *
 * ■ 補足
 *   本クラスは Composition Root として振る舞い、
 *   依存関係の組み立てを一元管理する。
 */
public partial class App : Application
{
    // Generic Host 本体
    private IHost? _host;

    // メインウィンドウ参照
    private Window? _mainWindow;

    /**
     * 現在のアプリケーション Host インスタンスを返す。
     *
     * Host が未初期化の場合は使用誤りとして例外を送出する。
     */
    public static IHost HostInstance =>
        (Current as App)?._host
        ?? throw new InvalidOperationException("Host is not initialized.");

    /**
     * DI コンテナ経由でサービス解決を行うための入口を返す。
     *
     * 本プロパティは Host が初期化済みであることを前提とする。
     */
    public static IServiceProvider Services => HostInstance.Services;

    /**
     * アプリケーションを初期化し、Host を構築する。
     *
     * この段階ではまだ Window は生成せず、
     * 起動に必要なサービス登録とホスト構築のみを行う。
     */
    public App()
    {
        InitializeComponent();

        _host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .UseSerilog((context, services, configuration) =>
            {
                // ファイル出力を行う Serilog を初期化
                configuration
                    .MinimumLevel.Debug()
                    .WriteTo.File(
                        path: "logs/app-.log",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14);
            })
            .ConfigureServices((context, services) =>
            {
                // DI コンテナへアプリケーションサービスを登録
                services.AddAppServices();
            })
            .Build();
    }

    /**
     * アプリケーション起動時に呼び出される。
     *
     * Host を起動した後、DI コンテナから ShellWindow を解決し、
     * メインウィンドウとして表示する。
     */
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        if (_host is null)
        {
            throw new InvalidOperationException("Host is not available.");
        }

        // Host を起動し、登録済みサービスを利用可能にする
        await _host.StartAsync();

        // DI コンテナから ShellWindow を取得
        _mainWindow = Services.GetRequiredService<ShellWindow>();

        // メインウィンドウを表示
        _mainWindow.Activate();

        // 先に DB 初期化を実行する
        var dbInitializer = Services.GetRequiredService<AppDbContextInitializer>();
        await dbInitializer.InitializeAsync();

        // 動作確認用に 1 件保存
        var repository = Services.GetRequiredService<IOperationLogRepository>();
        await repository.SaveAsync(new OperationLogEntity
        {
            Timestamp = DateTimeOffset.Now,
            LogType = "UI",
            EventCode = "APP_START",
            Result = "SUCCESS",
            Message = "Application started."
        });
    }
}