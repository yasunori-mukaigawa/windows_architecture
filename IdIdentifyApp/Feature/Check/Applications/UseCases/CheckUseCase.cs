using IdIdentifyApp.Feature.Check.Applications.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Feature.Check.Applications.UseCases;

/**
 * Check 機能で利用する UseCase 群を束ねるクラス。
 *
 * 本クラスは、Check 機能で利用する個別 UseCase をまとめて保持し、
 * ViewModel へ一括で注入するために使用する。
 *
 * ■ 提供する責務
 *   CheckPage1 用メッセージ取得 UseCase の提供
 *   CheckPage1_1 用メッセージ取得 UseCase の提供
 *
 * ■ 設計上の意図
 *   ViewModel へ個別 UseCase を複数注入すると依存が散らばるため、
 *   機能単位で UseCase 群を束ねて見通しを良くする。
 *
 * ■ 運用ルール
 *   Check 機能で新たな UseCase を追加した場合は、
 *   本クラスにプロパティを追加して集約する。
 */
public sealed class CheckUseCases
{
    /**
     * CheckPage1 表示用メッセージ取得 UseCase。
     */
    public GetCheckMessageUseCase GetCheckMessage { get; }

    /**
     * CheckPage2 表示用メッセージ取得 UseCase。
     */
    public GetCheck1_1MessageUseCase GetCheck1_1Message { get; }

    public CheckUseCases(
        GetCheckMessageUseCase getCheckMessage,
        GetCheck1_1MessageUseCase getCheck1_1Message)
    {
        GetCheckMessage = getCheckMessage;
        GetCheck1_1Message = getCheck1_1Message;
    }
}

/**
 * Check 機能で利用する UseCase 群を束ねるクラス。
 *
 * 本クラスは、Check 機能で利用する個別 UseCase をまとめて保持し、
 * ViewModel へ一括で注入するために使用する。
 *
 * ■ 提供する責務
 *   CheckPage2 用メッセージ取得 UseCase の提供
 *   CheckPage2_1 用メッセージ取得 UseCase の提供
 *
 * ■ 設計上の意図
 *   ViewModel へ個別 UseCase を複数注入すると依存が散らばるため、
 *   機能単位で UseCase 群を束ねて見通しを良くする。
 *
 * ■ 運用ルール
 *   Check 機能で新たな UseCase を追加した場合は、
 *   本クラスにプロパティを追加して集約する。
 */
public sealed class Check2UseCases
{
    /**
     * CheckPage1 表示用メッセージ取得 UseCase。
     */
    public GetCheck2MessageUseCase GetCheck2Message { get; }

    /**
     * CheckPage2 表示用メッセージ取得 UseCase。
     */
    public GetCheck2_1MessageUseCase GetCheck2_1Message { get; }

    public Check2UseCases(
        GetCheck2MessageUseCase getCheckMessage,
        GetCheck2_1MessageUseCase getCheck2_1Message)
    {
        GetCheck2Message = getCheckMessage;
        GetCheck2_1Message = getCheck2_1Message;
    }
}

/**
 * CheckPage1 表示用メッセージ取得 UseCase。
 *
 * 本 UseCase は Repository から必要なデータを取得し、
 * ViewModel が扱いやすい値として返す。
 */
public sealed class GetCheckMessageUseCase
{
    private readonly ICheckRepository _checkRepository;

    public GetCheckMessageUseCase(ICheckRepository checkRepository)
    {
        _checkRepository = checkRepository;
    }

    /**
     * 表示用メッセージを取得する。
     */
    public Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _checkRepository.GetMessageAsync(cancellationToken);
    }
}

/**
 * CheckPage1 表示用メッセージ取得 UseCase。
 *
 * 本 UseCase は Repository から必要なデータを取得し、
 * ViewModel が扱いやすい値として返す。
 */
public sealed class GetCheck1_1MessageUseCase
{
    private readonly ICheckRepository _checkRepository;

    public GetCheck1_1MessageUseCase(ICheckRepository checkRepository)
    {
        _checkRepository = checkRepository;
    }

    /**
     * 表示用メッセージを取得する。
     */
    public Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _checkRepository.GetMessageAsync(cancellationToken);
    }
}

/**
 * CheckPage2 表示用メッセージ取得 UseCase。
 *
 * 本 UseCase は Repository から必要なデータを取得し、
 * ViewModel が扱いやすい値として返す。
 */
public sealed class GetCheck2MessageUseCase
{
    private readonly ICheckRepository _checkRepository;

    public GetCheck2MessageUseCase(ICheckRepository checkRepository)
    {
        _checkRepository = checkRepository;
    }

    /**
     * 表示用メッセージを取得する。
     */
    public Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _checkRepository.GetMessage2Async(cancellationToken);
    }
}

/**
 * CheckPage2 表示用メッセージ取得 UseCase。
 *
 * 本 UseCase は Repository から必要なデータを取得し、
 * ViewModel が扱いやすい値として返す。
 */
public sealed class GetCheck2_1MessageUseCase
{
    private readonly ICheckRepository _checkRepository;

    public GetCheck2_1MessageUseCase(ICheckRepository checkRepository)
    {
        _checkRepository = checkRepository;
    }

    /**
     * 表示用メッセージを取得する。
     */
    public Task<string> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return _checkRepository.GetMessage2Async(cancellationToken);
    }
}