namespace IdIdentifyApp.Common.Ui.UiStates
{
    /**
     * UI の状態（State）を表す基底型。
     *
     * 本クラスは、画面に表示されるデータや状態を
     * 不変オブジェクトとして保持するための抽象型である。
     *
     * ■ 提供する責務
     *   UI 状態の型定義
     *   Reducer による状態遷移の対象となるデータ構造の提供
     *
     * ■ 設計上の意図
     *   State を不変（immutable）として扱うことで、
     *   状態変更の追跡性とテスト容易性を向上させる。
     */
    public abstract record BaseUiState;

    /**
     * 汎用的な初期状態を表す State。
     *
     * 本クラスは、画面初期表示時や
     * リセット時に使用する最小構成の State を提供する。
     *
     * ■ 提供する責務
     *   初期状態の定義
     *   State の雛形としての役割
     *
     * ■ 利用例
     *   - 画面初期表示
     *   - 処理リセット時
     *
     * ■ 注意
     *   実際の画面では、本クラスを継承して
     *   業務に応じたプロパティを追加すること。
     */
    public sealed record BaseState() : BaseUiState
    {
        /**
         * 初期状態を返す。
         *
         * ■ 設計上の意図
         *   初期値生成ロジックを 1 箇所に集約し、
         *   ViewModel 側の記述を簡潔にする。
         */
        public static BaseState Initial =>
            new();
    }
}