using IdIdentifyApp.Common.Ui.Intents;

namespace IdIdentifyApp.Applications.Feature.Check.Intents;

/**
 * CheckPage1 で発生する View 起点イベントの基底型。
 *
 * View は UI 操作を直接 ViewModel メソッドへ渡さず、
 * Intent として ViewModel へ publish する。
 */
public abstract record CheckPage1Intent : BaseUiIntent;

/**
 * 状態変更ボタン押下を表す Intent。
 */
public sealed record ChangeStateClicked() : CheckPage1Intent;

/**
 * データ取得ボタン押下を表す Intent。
 */
public sealed record LoadClicked() : CheckPage1Intent;

/**
 * 画面遷移ボタン押下を表す Intent。
 */
public sealed record NavigateClicked() : CheckPage1Intent;