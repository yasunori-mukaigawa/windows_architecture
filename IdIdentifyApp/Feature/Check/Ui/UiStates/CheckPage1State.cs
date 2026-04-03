using IdIdentifyApp.Common.Ui.UiStates;

namespace IdIdentifyApp.Feature.Check.Ui.UiStates;

/**
 * CheckPage1 の画面状態を表す。
 *
 * 本 State は、画面上の表示内容と操作可否を保持する。
 */
public sealed record CheckPage1State(
    string Title,
    string StatusMessage,
    string LoadedMessage,
    int Counter,
    bool CanNavigate,
    bool IsLoading) : BaseUiState
{
    /**
     * 初期状態を返す。
     */
    public static CheckPage1State Initial =>
        new(
            Title: "Check Page 1",
            StatusMessage: "初期状態です",
            LoadedMessage: "未取得",
            Counter: 0,
            CanNavigate: false,
            IsLoading: false);
}