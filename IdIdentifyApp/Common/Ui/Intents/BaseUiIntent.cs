namespace IdIdentifyApp.Common.Ui.Intents
{
    public abstract record BaseUiIntent;

    public abstract record BaseIntent : BaseUiIntent;

    /**
     * 状態変更ボタン押下を表す Intent。
     */
    public sealed record BaseAction() : BaseIntent;
}
