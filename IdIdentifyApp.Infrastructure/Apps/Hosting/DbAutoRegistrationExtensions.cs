using IdIdentifyApp.Common.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdIdentifyApp.Infrastructure.Apps.Hosting;

/**
 * DbContext の規約ベース自動登録を行う拡張クラス。
 *
 * 本クラスは、DbContext 継承型を Assembly から検出し、
 * 型名に基づく SQLite ファイル名で DbContextFactory を自動登録する。
 *
 * ■ ファイル名規約
 *   IdIdentifyDatabase -> ididentify.db
 *   OperationLogDatabase -> operationlog.db
 *   SampleDbContext -> sample.db
 */
public static class DbAutoRegistrationExtensions
{
    /**
     * 指定 Assembly から DbContext を検出し、自動登録する。
     */
    public static IServiceCollection AddSqliteDbContextsFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        string dataDirectory)
    {
        Directory.CreateDirectory(dataDirectory);

        var dbContextTypes = assembly
            .GetTypes()
            .Where(t =>
                t is { IsClass: true, IsAbstract: false } &&
                typeof(DbContext).IsAssignableFrom(t))
            .ToArray();

        // 初期化用に対象一覧を保持する
        services.AddSingleton(new DbContextRegistry(dbContextTypes));

        foreach (var dbContextType in dbContextTypes)
        {
            RegisterDbContextFactory(services, dbContextType, dataDirectory);
        }

        return services;
    }

    /**
     * 指定 DbContext 型の DbContextFactory を登録する。
     */
    private static void RegisterDbContextFactory(
        IServiceCollection services,
        Type dbContextType,
        string dataDirectory)
    {
        var dbFileName = ToDbFileName(dbContextType.Name);
        var dbPath = Path.Combine(dataDirectory, dbFileName);

        var method = typeof(DbAutoRegistrationExtensions)
            .GetMethod(nameof(RegisterDbContextFactoryGeneric), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(dbContextType);

        method.Invoke(null, new object[] { services, dbPath });
    }

    /**
     * DbContextFactory を登録するジェネリック実装。
     */
    private static void RegisterDbContextFactoryGeneric<TContext>(
        IServiceCollection services,
        string dbPath)
        where TContext : DbContext
    {
        services.AddDbContextFactory<TContext>(options =>
        {
            // SQLite を利用する
            options.UseSqlite($"Data Source={dbPath}");
        });
    }

    /**
     * DbContext 型名から DB ファイル名を生成する。
     */
    private static string ToDbFileName(string typeName)
    {
        var name = typeName;

        if (name.EndsWith("Database", StringComparison.Ordinal))
        {
            name = name[..^"Database".Length];
        }
        else if (name.EndsWith("DbContext", StringComparison.Ordinal))
        {
            name = name[..^"DbContext".Length];
        }

        return $"{name.ToLowerInvariant()}.db";
    }
}