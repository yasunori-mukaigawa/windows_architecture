namespace IdIdentifyApp.Applications.Feature.Check.Ports;

/**
 * Check 動作確認用データ取得 Repository 契約。
 *
 * 本契約は、UseCase が表示用の確認データを取得するために使用する。
 */
public interface ICheckRepository
{
    /**
     * 表示用メッセージを取得する。
     */
    Task<string> GetMessageAsync(CancellationToken cancellationToken = default);

    /**
     * 表示用メッセージを取得する。
     */
    Task<string> GetMessage2Async(CancellationToken cancellationToken = default);
}