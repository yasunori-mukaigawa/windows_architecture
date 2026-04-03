using System;
using System.Collections.Generic;
using System.Linq;

namespace IdIdentifyApp.Common.Infrastructure.Db;

/**
 * 自動登録された DbContext 型一覧を保持するレジストリ。
 *
 * 本クラスは、起動時 DB 初期化処理で
 * どの DbContext を対象にするかを判定するために使用する。
 */
public sealed class DbContextRegistry
{
    /**
     * 自動登録対象の DbContext 型一覧。
     */
    public IReadOnlyList<Type> DbContextTypes { get; }

    public DbContextRegistry(IEnumerable<Type> dbContextTypes)
    {
        DbContextTypes = dbContextTypes.ToArray();
    }
}