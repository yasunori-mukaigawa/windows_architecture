
using IdIdentifyApp.Applications;
using Microsoft.Extensions.DependencyInjection;

/**
 * UseCase の DI 自動登録拡張。
 *
 * 本クラスは、Application 層に定義された Reducer を
 * 命名規約に基づいて自動登録する責務を持つ。
 *
 * ■ 提供する責務
 *   Reducer クラスの自動検出と登録
 *   DI コンテナへの Transient ライフタイムでの登録
 *
 * ■ 設計上の意図
 *   Reducer の追加時に個別登録コードを不要とし、
 *   命名規約（〜Reducer）に従うことで
 *   登録の一貫性と保守性を向上させる。
 *
 * ■ 注意
 *   スキャン対象は Application 層の Assembly に限定すること。
 *   App 全体をスキャンすると、レイヤ分離が崩れるため避ける。
 */
public static class ReducerServiceCollectionExtensions
{
    /**
     * UseCase を DI コンテナへ登録する。
     *
     * ■ 登録対象
     *   - "Reducer" で終わるクラス
     *
     * ■ ライフタイム
     *   Transient（Reducer はステートを持たないため）
     *
     * ■ 実装詳細
     *   Scrutor の Scan を利用し、
     *   指定 Assembly 内のクラスを命名規約ベースで登録する。
     */
    public static IServiceCollection AddReducers(this IServiceCollection services)
    {
        // Reducer 登録 
        // 命名規約: ～Reducer
        services.Scan(scan => scan
            // Application 層の Assembly を起点にスキャンする
            .FromAssemblyOf<AppAssemblyMarker>()
            // Reducer クラスのみ抽出する
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Reducer")))
            // 自身の型として登録（interface を強制しない設計）
            .AsSelf()
            // ステートレスのため Transient で登録
            .WithTransientLifetime());

        return services;
    }
}