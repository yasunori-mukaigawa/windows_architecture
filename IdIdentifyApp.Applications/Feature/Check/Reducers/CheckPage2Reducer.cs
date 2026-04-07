using IdIdentifyApp.Applications.Feature.Check.Messages;
using IdIdentifyApp.Applications.Feature.Check.UiStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdIdentifyApp.Applications.Feature.Check.Reducers
{
    /**
     * CheckPage2 用の Reducer。
     *
     * 本クラスは、CheckPage2 で発生する Message を受け取り、
     * 次の State を生成する責務を持つ。
     *
     * ■ 提供する責務
     *   Message に応じた State 遷移
     *   UI表示状態の計算
     *
     * ■ 設計上の意図
     *   状態遷移ロジックを ViewModel から分離し、
     *   テスト容易性と責務分離を向上させる。
     *
     * ■ 注意
     *   Reducer は純粋関数として振る舞うこと。
     *   外部呼び出しや副作用を持ち込んではならない。
     */
    public class CheckPage2Reducer
    {

        public CheckPage2State Reduce(CheckPage2State currentState, CheckPage2Message message)
        {
            return message switch
            {
                LoadStarted2 =>
                    currentState with
                    {
                        IsCompleted = false,
                        StatusMessage = "データ取得中です"
                    },

                LoadSucceeded2 succeeded =>
                    currentState with
                    {
                        IsCompleted = true,
                        StatusMessage = succeeded.Message,
                    },

                LoadFailed2 failed =>
                    currentState with
                    {
                        IsCompleted = true,
                        StatusMessage = failed.Error.UserMessage,
                    },

                _ => currentState
            };
        }
    }
}
