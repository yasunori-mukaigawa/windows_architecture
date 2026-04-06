namespace IdIdentifyApp.Common.Ui.Intents
{
    /**
     * UI から発行される操作意図（Intent）の基底型。
     *
     * 本クラスは、ユーザー操作や外部イベントなど、
     * 「何をしたいか」という意図を表現するための抽象型である。
     *
     * ■ 提供する責務
     *   UI 操作の入力を型安全に表現する
     *   ViewModel への入力イベントを統一する
     *
     * ■ 設計上の意図
     *   View は「状態変更」ではなく「意図（Intent）」を送ることで、
     *   UI と状態管理ロジックの分離を実現する。
     */
    public abstract record BaseUiIntent;

    /**
     * アプリケーション内で扱う標準 Intent の基底型。
     *
     * 本クラスは、BaseUiIntent を継承し、
     * アプリ内で使用する Intent の共通基底として機能する。
     *
     * ■ 設計上の意図
     *   将来的に Intent を種類別に分割する際の拡張ポイントとして使用する。
     */
    public abstract record BaseIntent : BaseUiIntent;

    /**
     * 汎用アクションを表す Intent。
     *
     * 本クラスは、ボタン押下などの単純なユーザー操作を
     * 表現するための最小単位の Intent である。
     *
     * ■ 提供する責務
     *   ユーザー操作（例: ボタン押下）を表現する
     *
     * ■ 利用例
     *   - 「次へ」ボタン押下
     *   - 「実行」ボタン押下
     *
     * ■ 注意
     *   実際の業務処理に紐づく Intent は、
     *   本クラスを継承して個別に定義すること。
     */
    public sealed record BaseAction() : BaseIntent;
}