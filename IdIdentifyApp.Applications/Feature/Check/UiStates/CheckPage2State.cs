using IdIdentifyApp.Common.Ui.UiStates;

namespace IdIdentifyApp.Applications.Feature.Check.UiStates;

/**
 * CheckPage2 の画面状態を表す。
 *
 * 動作確認用として、Page2 側でも State 更新に応じて
 * UI が変化することを確認するために使用する。
 */
public sealed record CheckPage2State(
    string Title,
    string StatusMessage,
    bool IsCompleted) : BaseUiState
{
    /**
     * 初期状態を返す。
     */
    public static CheckPage2State Initial =>
        new(
            Title: "Check Page 2",
            StatusMessage: "まだ完了していません",
            IsCompleted: false);
}