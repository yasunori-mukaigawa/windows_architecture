using IdIdentifyApp.Common.Ui.Messages;

namespace IdIdentifyApp.Feature.Check.Ui.Messages;

/**
 * CheckPage2 で扱う Message の基底型。
 */
public abstract record CheckPage2Message : BaseUiMessage;

/**
 * UseCase から表示メッセージの取得を開始したことを表す Message。
 */
public sealed record LoadStarted2() : CheckPage2Message;

/**
 * UseCase から表示メッセージの取得に成功したことを表す Message。
 */
public sealed record LoadSucceeded2(string Message) : CheckPage2Message;

/**
 * UseCase からの取得に失敗したことを表す Message。
 */
public sealed record LoadFailed2(string ErrorMessage) : CheckPage2Message;