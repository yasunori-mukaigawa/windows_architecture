using IdIdentifyApp.Applications.Feature.Check.UseCases;
using IdIdentifyApp.Common.Domain.Error;
using IdIdentifyApp.Common.Ui.Mvi;
using IdIdentifyApp.Common.Ui.ViewModels;
using IdIdentifyApp.Feature.Check.Ui.Intents;
using IdIdentifyApp.Feature.Check.Ui.Messages;
using IdIdentifyApp.Feature.Check.Ui.UiStates;
using IdIdentifyApp.Feature.Check.Ui.Views;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Feature.Check.Ui.ViewModels;

/**
 * CheckPage1 用の Vieodel。
 *
 * 本 ViewModel は以下の確認を目的とする。
 * ・State 更新に応じて UI 表示が変わること
 * ・UseCase / Repository から取得した値を State に反映すること
 * ・State 更新後に Effect を発行し、画面遷移できること
 *
 * ■ 設計上の意図
 *   View からの入力は Intent として受け取り、
 *   意味解釈、State 更新、UseCase 呼び出し、Effect 発火を本 ViewModel で一元化する。
 */
public sealed class CheckPage1ViewModel
    : AbsViewModel<CheckPage1State, CheckPage1Message, CheckPage1Intent>
{
    private readonly CheckUseCases _checkUseCases;

    public CheckPage1ViewModel(CheckUseCases checkUseCases)
        : base(CheckPage1State.Initial)
    {
        _checkUseCases = checkUseCases;
    }

    /**
     * Reducer。
     *
     * Message に応じて次の State を生成する。
     */
    protected override CheckPage1State Reduce(CheckPage1State currentState, CheckPage1Message message)
    {
        return message switch
        {
            ChangeStateRequested =>
                currentState with
                {
                    StatusMessage = $"状態が更新されました（{currentState.Counter + 1}回目）",
                    Counter = currentState.Counter + 1,
                    CanNavigate = true
                },

            LoadStarted =>
                currentState with
                {
                    IsLoading = true,
                    StatusMessage = "データ取得中です"
                },

            LoadSucceeded succeeded =>
                currentState with
                {
                    IsLoading = false,
                    LoadedMessage = succeeded.Message,
                    StatusMessage = "データ取得に成功しました"
                },

            LoadFailed failed =>
                currentState with
                {
                    IsLoading = false,
                    LoadedMessage = failed.Error.UserMessage,
                    StatusMessage = "データ取得に失敗しました"
                },

            _ => currentState
        };
    }

    /**
     * Intent の意味解釈を行う。
     *
     * View から受け取った Intent を起点に、
     * Message 生成、UseCase 呼び出し、Effect 発火を行う。
     */
    protected override async Task HandleIntentAsync(CheckPage1Intent intent, CancellationToken cancellationToken)
    {
        switch (intent)
        {
            case ChangeStateClicked:
                // 状態変更要求を Message 化する
                Dispatch(new ChangeStateRequested());
                break;

            case LoadClicked:
                await HandleLoadAsync(cancellationToken);
                break;

            case NavigateClicked:
                await HandleNavigateAsync(cancellationToken);
                break;

            default:
                throw new NotSupportedException($"Unsupported intent type: {intent.GetType().FullName}");
        }
    }

    /**
     * データ取得 Intent を処理する。
     *
     * UseCase を呼び出し、取得結果を Message 化して State に反映する。
     * 失敗時は DomainError の内容を UI 向けに反映する。
     */
    private async Task HandleLoadAsync(CancellationToken cancellationToken)
    {
        Dispatch(new LoadStarted());

        var result = await _checkUseCases.GetCheckMessage.ExecuteAsync(cancellationToken);

        if (result.IsSuccess)
        {
            Dispatch(new LoadSucceeded(result.Value!));
            return;
        }

        var error = result.Error!;

        Dispatch(new LoadFailed(error));

        await PublishLoadErrorEffectAsync(error, cancellationToken);
    }

    /**
     * 取得失敗時の Effect 発火を行う。
     *
     * Error の復旧属性に応じて、ダイアログ文言を切り替える。
     */
    private async Task PublishLoadErrorEffectAsync(DomainError error, CancellationToken cancellationToken)
    {
        var message = BuildRecoveryGuidance(error);

        await PublishEffectAsync(
            new ShowDialogEffect(error.Title, message),
            cancellationToken);
    }

    /**
     * 画面遷移 Intent を処理する。
     *
     * 遷移条件を満たさない場合はダイアログ表示 Effect を発行する。
     */
    private async Task HandleNavigateAsync(CancellationToken cancellationToken)
    {
        if (!State.CanNavigate)
        {
            await PublishEffectAsync(
                new ShowDialogEffect("遷移不可", "先に状態変更を実行してください。"),
                cancellationToken);

            return;
        }

        // 条件を満たしているため Page2 への遷移を要求
        await PublishEffectAsync(
            new NavigateToEffect(typeof(CheckPage2)),
            cancellationToken);
    }
}