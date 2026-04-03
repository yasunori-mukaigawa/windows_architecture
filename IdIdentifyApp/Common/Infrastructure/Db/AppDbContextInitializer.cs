using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Infrastructure.Db;

/**
 * アプリケーション共通 DB 初期化処理。
 *
 * 本クラスは、起動時に自動登録された DbContext を走査し、
 * DB 作成やマイグレーション適用を行う責務を持つ。
 *
 * ■ 提供する責務
 *   自動登録済み DbContext の列挙
 *   DB 初期作成
 *   将来的なマイグレーション適用
 *
 * ■ 設計上の意図
 *   DB 初期化処理を起動処理や Repository から分離し、
 *   初期化責務を一元化する。
 */
public sealed class AppDbContextInitializer
{
    // DI コンテナから各 DbContextFactory を取得するための ServiceProvider
    private readonly IServiceProvider _serviceProvider;

    // 自動登録対象の DbContext 一覧
    private readonly DbContextRegistry _dbContextRegistry;

    public AppDbContextInitializer(
        IServiceProvider serviceProvider,
        DbContextRegistry dbContextRegistry)
    {
        _serviceProvider = serviceProvider;
        _dbContextRegistry = dbContextRegistry;
    }

    /**
     * 登録済みの全 DbContext を初期化する。
     */
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbContextType in _dbContextRegistry.DbContextTypes)
        {
            await InitializeDbContextAsync(dbContextType, cancellationToken);
        }
    }

    /**
     * 指定 DbContext 型の初期化処理を実行する。
     */
    private Task InitializeDbContextAsync(Type dbContextType, CancellationToken cancellationToken)
    {
        var method = typeof(AppDbContextInitializer)
            .GetMethod(nameof(InitializeDbContextCoreAsync), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(dbContextType);

        return (Task)(method.Invoke(this, new object[] { cancellationToken })
            ?? throw new InvalidOperationException(
                $"Failed to invoke initializer for DbContext type {dbContextType.FullName}."));
    }

    /**
     * 指定 DbContext 型の DB 初期化を実行する。
     */
    private async Task InitializeDbContextCoreAsync<TContext>(CancellationToken cancellationToken)
        where TContext : DbContext
    {
        var factory = _serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();

        await using var dbContext = await factory.CreateDbContextAsync(cancellationToken);

        // 開発初期用の簡易初期化
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        // Migration 運用へ切り替える場合は以下を使用する
        // await dbContext.Database.MigrateAsync(cancellationToken);
    }
}