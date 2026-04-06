using IdIdentifyApp.Applications.Feature.Check.Ports;

namespace IdIdentifyApp.Infrastructure.Feature.Check.Repositories;

/**
 * Check 動作確認用の Mock Repository 実装。
 *
 * 本実装は、実DBや外部接続を行わず固定値を返す。
 * ViewModel → UseCase → Repository のデータフロー確認を目的とする。
 */
public sealed class MockCheckRepository : ICheckRepository
{
    /**
     * モックメッセージを返す。
     */
    public async Task<string> GetMessageAsync(CancellationToken cancellationToken = default)
    {
        // 疑似的に非同期処理らしさを出す
        await Task.Delay(200, cancellationToken);

        return "UseCase / Repository から取得したメッセージです";
    }

    /**
 * モックメッセージを返す。
 */
    public async Task<string> GetMessage2Async(CancellationToken cancellationToken = default)
    {
        // 疑似的に非同期処理らしさを出す
        await Task.Delay(200, cancellationToken);

        return "UseCase / Repository から取得\"完了状態になりました\"";
    }
}