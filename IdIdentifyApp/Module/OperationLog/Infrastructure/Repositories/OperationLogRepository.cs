using IdIdentifyApp.Modules.OperationLog.Application.Ports;
using IdIdentifyApp.Modules.OperationLog.Domain.Entities;
using IdIdentifyApp.Modules.OperationLog.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Modules.OperationLog.Infrastructure.Repositories;

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