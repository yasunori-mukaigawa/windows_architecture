using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdIdentifyApp.Common.Apps.Hosting;

/**
 * モジュールサービス登録実行拡張。
 *
 * 本クラスは、指定アセンブリ内から
 * IModuleServiceRegistrar 実装を探索し、
 * 各モジュールの登録処理を実行する。
 *
 * ■ 提供する責務
 *   registrar 実装クラス探索
 *   registrar 実行
 *
 * ■ 設計上の意図
 *   Common 層にモジュール登録の起点を集約しつつ、
 *   Common → Module の静的依存を発生させない構成とする。
 *
 * ■ 注意
 *   registrar 実装には引数なしコンストラクタが必要である。
 */
public static class ModuleServiceCollectionExtensions
{
    /**
     * 指定アセンブリ群からモジュールサービス登録を実行する。
     */
    public static IServiceCollection AddModuleServices(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var registrarTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type =>
                typeof(IModuleServiceRegistrar).IsAssignableFrom(type) &&
                type is { IsClass: true, IsAbstract: false });

        foreach (var registrarType in registrarTypes)
        {
            var registrar = (IModuleServiceRegistrar)Activator.CreateInstance(registrarType)!;
            registrar.Register(services);
        }

        return services;
    }
}