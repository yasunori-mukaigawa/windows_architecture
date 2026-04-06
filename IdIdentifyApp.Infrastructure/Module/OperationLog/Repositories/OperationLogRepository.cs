using IdIdentifyApp.Applications.Module.OperationLog.Ports;
using IdIdentifyApp.Domain.Module.OperationLog.Entity;
using IdIdentifyApp.Infrastructure.Module.OperationLog.Db;
using Microsoft.EntityFrameworkCore;

namespace IdIdentifyApp.Infrastructure.Module.OperationLog.Repositories;

/**
 * 操作ログ Repository 実装。
 *
 * 本クラスは、操作ログの保存を担当する。
 *
 * ■ 提供する責務
 *   操作ログ Entity の永続化
 *
 * ■ 設計上の意図
 *   UseCase や横断ログサービスから DB アクセス詳細を分離し、
 *   永続化責務を Repository に集約する。
 */
public sealed class OperationLogRepository : IOperationLogRepository
{
    // EF Core 標準の DbContextFactory
    private readonly IDbContextFactory<OperationLogDatabase> _dbContextFactory;

    public OperationLogRepository(IDbContextFactory<OperationLogDatabase> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    /**
     * 操作ログを保存する。
     */
    public async Task SaveAsync(OperationLogEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        // 操作ログを追加する
        dbContext.OperationLogs.Add(entity);

        // DB へ保存する
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}