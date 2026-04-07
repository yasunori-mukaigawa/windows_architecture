using IdIdentifyApp.Common.Ui.Intents;

namespace IdIdentifyApp.Applications.Feature.Check.Intents;

/**
 * CheckPage2 で発生する View 起点イベントの基底型。
 */
public abstract record CheckPage2Intent : BaseUiIntent;

/**
 * 完了状態変更ボタン押下を表す Intent。
 */
public sealed record CompleteClicked() : CheckPage2Intent;

/**
 * 戻るボタン押下を表す Intent。
 */
public sealed record BackClicked() : CheckPage2Intent;