
using IdIdentifyApp.Domain.Module.OperationLog.Entity;
using Microsoft.EntityFrameworkCore;

namespace IdIdentifyApp.Infrastructure.Module.OperationLog.Db;

/**
 * 操作ログ用 DbContext。
 *
 * 本 DbContext は、操作ログモジュールで利用する永続データを管理する。
 *
 * ■ 規約
 *   型名 "OperationLogDatabase" から、
 *   規約ベース自動登録により "operationlog.db" が生成される。
 *
 * ■ 設計上の意図
 *   操作ログを独立した DB として扱うことで、
 *   他機能のデータと物理的責務を分離しやすくする。
 *
 * ■ 注意
 *   操作ログテーブルのみを保持する。
 *   後続で error_log 等を追加する場合は本 DbContext に拡張する。
 */
public sealed class OperationLogDatabase : DbContext
{
    public OperationLogDatabase(DbContextOptions<OperationLogDatabase> options)
        : base(options)
    {
    }

    /**
     * 操作ログテーブル。
     */
    public DbSet<OperationLogEntity> OperationLogs => Set<OperationLogEntity>();

    /**
     * モデル定義を構成する。
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 操作ログ Entity のマッピング定義を適用する
        modelBuilder.ApplyConfiguration(new OperationLogEntityConfiguration());
    }
}