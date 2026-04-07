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
 * CheckPage1 用の Reducer。
 *
 * 本クラスは、CheckPage1 で発生する Message を受け取り、
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
    public class CheckPage1Reducer
    {

        public CheckPage1State Reduce(CheckPage1State currentState, CheckPage1Message message)
        {
            return message switch
            {
                ChangeStateRequested =>
                    currentState with
                    {
                        StatusMessage = $"状態が更新されました（{currentState.Counter + 1}回目）",
                        Counter = currentState.Counter + 1,
                        CanNavigate = true
                    },

                LoadStarted =>
                    currentState with
                    {
                        IsLoading = true,
                        StatusMessage = "データ取得中です"
                    },

                LoadSucceeded succeeded =>
                    currentState with
                    {
                        IsLoading = false,
                        LoadedMessage = succeeded.Message,
                        StatusMessage = "データ取得に成功しました"
                    },

                LoadFailed failed =>
                    currentState with
                    {
                        IsLoading = false,
                        LoadedMessage = failed.Error.UserMessage,
                        StatusMessage = "データ取得に失敗しました"
                    },

                _ => currentState
            };
        }
    }
}
