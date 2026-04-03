namespace IdIdentifyApp.Common.Ui.Mvi;

/**
 * One-shot UI Effect の基底型。
 *
 * 本型は、UiState に保持すべきではない単発の UI 操作要求を表現する。
 *
 * ■ 対象となる操作
 *   画面遷移
 *   ダイアログ表示
 *   トースト通知
 *   ファイル選択や印刷要求等の一度だけ実行すべき操作
 *
 * ■ 設計上の意図
 *   これらの操作を State に保持すると、
 *   再描画や再購読のタイミングで意図せず再実行される可能性がある。
 *   そのため、本システムでは継続状態である State と、
 *   単発通知である Effect を明確に分離する。
 *
 * ■ 運用ルール
 *   Effect は Channel を通じて一度だけ消費する
 *   Effect を UiState のフラグとして保持しない
 *   実際の UI 副作用の実行は View または Shell 側で行う
 */
public abstract record UiEffect;