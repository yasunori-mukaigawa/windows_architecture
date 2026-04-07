using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Ui.Intents;

/**
 * View から発生した Intent を受け付ける契約。
 *
 * View は ViewModel の具体メソッドを直接呼ばず、
 * 本契約を通じて Intent を通知する。
 */
public interface IIntentPublisher
{
    /**
     * 指定した Intent を発行する。
     *
     * Intent は ViewModel 内部の入力ストリームへ流され、
     * 順次処理される。
     */
    ValueTask PublishIntentAsync(object intent, CancellationToken cancellationToken = default);
}