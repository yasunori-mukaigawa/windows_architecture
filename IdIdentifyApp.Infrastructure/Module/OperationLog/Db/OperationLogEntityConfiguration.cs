using IdIdentifyApp.Domain.Module.OperationLog.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdIdentifyApp.Infrastructure.Module.OperationLog.Db;

/**
 * OperationLogEntity の EF Core マッピング定義。
 *
 * 本クラスは、操作ログ Entity と SQLite テーブル定義との対応付けを行う。
 *
 * ■ 提供する責務
 *   テーブル名定義
 *   主キー定義
 *   各列の型・必須制約・長さ制約の定義
 *
 * ■ 設計上の意図
 *   Entity 本体から永続化詳細を分離し、
 *   DB 定義変更時の責務を明確にする。
 */
public sealed class OperationLogEntityConfiguration : IEntityTypeConfiguration<OperationLogEntity>
{
    /**
     * Entity の永続化定義を構成する。
     */
    public void Configure(EntityTypeBuilder<OperationLogEntity> builder)
    {
        // テーブル名を定義
        builder.ToTable("operation_log");

        // 主キーを定義
        builder.HasKey(x => x.OperationLogId);

        // SQLite の自動採番列として扱う
        builder.Property(x => x.OperationLogId)
            .ValueGeneratedOnAdd();

        // 記録日時は必須
        builder.Property(x => x.Timestamp)
            .IsRequired();

        // ログ種別は必須、最大長を定義
        builder.Property(x => x.LogType)
            .HasMaxLength(32)
            .IsRequired();

        // イベントコードは必須、最大長を定義
        builder.Property(x => x.EventCode)
            .HasMaxLength(64)
            .IsRequired();

        // 実行結果は必須、最大長を定義
        builder.Property(x => x.Result)
            .HasMaxLength(32)
            .IsRequired();

        // メッセージは任意、最大長を定義
        builder.Property(x => x.Message)
            .HasMaxLength(1024);
    }
}