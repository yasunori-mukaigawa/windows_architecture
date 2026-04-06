using IdIdentifyApp.Feature.Check.Applications.UseCases;
using IdIdentifyApp.Feature.Check.Ui.Intents;
using IdIdentifyApp.Feature.Check.Ui.Messages;
using IdIdentifyApp.Feature.Check.Ui.UiStates;
using IdIdentifyApp.Common.Domain.Errors;
using IdIdentifyApp.Common.Ui.Mvi;
using IdIdentifyApp.Common.Ui.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Feature.Check.Ui.ViewModels;

/**
 * CheckPage2 用の ViewModel。
 *
 * 本 ViewModel は以下の確認を目的とする。
 * ・Page2 側でも State 更新に応じて UI が変わること
 * ・GoBackEffect により前画面へ戻れること
 */
public sealed class CheckPage2ViewModel
    : AbsViewModel<CheckPage2State, CheckPage2Message, CheckPage2Intent>
{
    private readonly Check2UseCases _check2UseCases;

    public CheckPage2ViewModel(Check2UseCases check2UseCases)
        : base(CheckPage2State.Initial)
    {
        _check2UseCases = check2UseCases;
    }

    /**
     * Reducer。
     *
     * Message に応じて次の State を生成する。
     */
    protected override CheckPage2State Reduce(CheckPage2State currentState, CheckPage2Message message)
    {
        return message switch
        {
            LoadStarted2 =>
                currentState with
                {
                    IsCompleted = false,
                    StatusMessage = "データ取得中です"
                },

            LoadSucceeded2 succeeded =>
                currentState with
                {
                    IsCompleted = true,
                    StatusMessage = succeeded.Message,
                },

            LoadFailed2 failed =>
                currentState with
                {
                    IsCompleted = true,
                    StatusMessage = failed.Error.UserMessage,
                },

            _ => currentState
        };
    }

    /**
     * Intent の意味解釈を行う。
     */
    protected override async Task HandleIntentAsync(CheckPage2Intent intent, CancellationToken cancellationToken)
    {
        switch (intent)
        {
            case CompleteClicked:
                // 完了状態へ遷移する
                await HandleLoadAsync(cancellationToken);
                break;

            case BackClicked:
                // 前画面へ戻ることを要求する
                await PublishEffectAsync(new GoBackEffect(), cancellationToken);
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
        Dispatch(new LoadStarted2());

        var result = await _check2UseCases.GetCheck2Message.ExecuteAsync(cancellationToken);

        if (result.IsSuccess)
        {
            Dispatch(new LoadSucceeded2(result.Value!));
            return;
        }

        var error = result.Error!;

        Dispatch(new LoadFailed2(error));

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
}