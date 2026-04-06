using System;

namespace IdIdentifyApp.Common.Ui.Mvi;

/**
 * 指定したページへの画面遷移要求を表す Effect。
 *
 * 本 Effect は、ViewModel が「どこへ遷移したいか」を通知するために使用する。
 * 実際の遷移処理は Shell または NavigationService 側で実行する。
 *
 * ■ 設計上の意図
 *   ViewModel から直接 Frame.Navigate を呼び出さず、
 *   UI固有処理を Shell 側へ分離することで責務を明確化する。
 *
 * ■ パラメータ
 *   PageType  : 遷移先のページ型
 *   Parameter : 遷移時に渡す任意のパラメータ
 */
public sealed record NavigateToEffect(Type PageType, object? Parameter = null) : UiEffect;

/**
 * 1つ前の画面へ戻る要求を表す Effect。
 *
 * 本 Effect は、戻る操作を要求するだけであり、
 * 実際に戻れるかどうかの判定と実行は NavigationService 側で行う。
 */
public sealed record GoBackEffect() : UiEffect;

/**
 * ダイアログ表示要求を表す Effect。
 *
 * 本 Effect は、ユーザーへ明示的な通知や確認を行うための
 * ダイアログ表示を要求する。
 *
 * ■ パラメータ
 *   Title   : ダイアログタイトル
 *   Message : ダイアログ本文
 */
public sealed record ShowDialogEffect(string Title, string Message) : UiEffect;

/**
 * 軽量な通知表示要求を表す Effect。
 *
 * 本 Effect は、エラーではない簡易通知や操作完了通知等を
 * ユーザーへ一度だけ表示したい場合に使用する。
 *
 * ■ 補足
 *   実際の表示方法は実装側に委ねる。
 *   初期段階ではダイアログで代替してもよいが、
 *   最終的には InfoBar や InAppNotification などへ置き換えることを想定する。
 *
 * ■ パラメータ
 *   Message : 表示する通知メッセージ
 */
public sealed record ShowToastEffect(string Message) : UiEffect;