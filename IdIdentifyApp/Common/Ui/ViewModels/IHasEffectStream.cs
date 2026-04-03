using System.Threading.Channels;
using IdIdentifyApp.Common.Ui.Mvi;

namespace IdIdentifyApp.Common.Ui.ViewModels;

/**
 * Shell が ViewModel の Effect ストリームを購読するための共通契約。
 *
 * ViewModel の具体型を知らなくても、
 * One-shot Effect を購読できるようにすることを目的とする。
 */
public interface IHasEffectStream
{
    /**
     * One-shot Effect の購読用 Reader を返す。
     */
    ChannelReader<UiEffect> EffectReader { get; }
}