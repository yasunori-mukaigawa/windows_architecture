using IdIdentifyApp.Common.Ui.Intents;
using IdIdentifyApp.Feature.Check.Ui.Intents;
using IdIdentifyApp.Feature.Check.Ui.UiStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Ui.UiStates
{
    public abstract record BaseUiState;

    /**
     * 状態変更ボタン押下を表す Intent。
     */
    public sealed record BaseState() : BaseUiState
    {
        /**
         * 初期状態を返す。
         */
        public static BaseState Initial =>
            new();
    }
}
