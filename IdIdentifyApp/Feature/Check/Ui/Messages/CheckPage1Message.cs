using IdIdentifyApp.Common.Ui.Messages;

namespace IdIdentifyApp.Feature.Check.Ui.Messages;

/**
 * CheckPage1 で扱う Message の基底型。
 */
public abstract record CheckPage1Message : BaseUiMessage;

/**
 * 状態変更ボタン押下を表す Message。
 */
public sealed record ChangeStateRequested() : CheckPage1Message;

/**
 * 画面遷移可能状態になったことを表す Message。
 */
public sealed record NavigationEnabled() : CheckPage1Message;

/**
 * UseCase から表示メッセージの取得を開始したことを表す Message。
 */
public sealed record LoadStarted() : CheckPage1Message;

/**
 * UseCase から表示メッセージの取得に成功したことを表す Message。
 */
public sealed record LoadSucceeded(string Message) : CheckPage1Message;

/**
 * UseCase からの取得に失敗したことを表す Message。
 */
public sealed record LoadFailed(string ErrorMessage) : CheckPage1Message;