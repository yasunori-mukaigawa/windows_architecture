using IdIdentifyApp.Common.Ui.UiStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Ui.Messages
{
    public abstract record BaseUiMessage;

    public abstract record BaseMessage : BaseUiMessage;

    /**
     * 状態変更ボタン押下を表す Intent。
     */
    public sealed record BaseAction() : BaseMessage;
}
