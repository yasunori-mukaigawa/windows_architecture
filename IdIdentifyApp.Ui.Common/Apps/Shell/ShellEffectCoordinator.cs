using IdIdentifyApp.Common.Ui.Mvi;
using IdIdentifyApp.Common.Ui.ViewModels;
using IdIdentifyApp.Ui.Common.Apps.Navigation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IdIdentifyApp.Ui.Common.Apps.Shell;

/**
 * Shell における Navigation 初期化および
 * Effect の購読・解釈を担当する共通コーディネータ。
 *
 * 本クラスは、ShellWindow から
 * 画面遷移制御と One-shot Effect 処理を分離し、
 * UI 共通基盤として再利用できるようにする。
 *
 * ■ 提供する責務
 *   RootFrame を NavigationService へ接続する
 *   ページ遷移後の Effect 購読張り替えを補助する
 *   UiEffect を解釈し、Navigation / Dialog 表示要求へ変換する
 *
 * ■ 設計上の意図
 *   ShellWindow には「アプリ固有の組み立て」だけを残し、
 *   共通化可能な制御ロジックを本クラスへ集約する。
 *
 * ■ 注意
 *   ContentDialog の実表示や初期画面決定など、
 *   アプリ固有判断は呼び出し側で行う。
 */
public sealed class ShellEffectCoordinator
{
    private readonly INavigationService _navigationService;

    public ShellEffectCoordinator(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    /**
     * Shell が保持する RootFrame を NavigationService に接続する。
     *
     * 以降の Navigate / GoBack は本 Frame を対象に実行される。
     */
    public void Initialize(Frame rootFrame)
    {
        _navigationService.Initialize(rootFrame);
    }

    /**
     * 現在表示中ページから Effect 発行元を取得する。
     *
     * DataContext が IHasEffectStream を実装している場合のみ返す。
     */
    public IHasEffectStream? FindEffectSource(object? pageContent)
    {
        if (pageContent is FrameworkElement element &&
            element.DataContext is IHasEffectStream effectSource)
        {
            return effectSource;
        }

        return null;
    }

    /**
     * ViewModel の EffectReader を順次購読し、
     * 受信した Effect を UI スレッド上で解釈・実行する。
     *
     * effectExecutor には、UI スレッドへ切り替えて
     * 実処理を実行するための委譲先を渡す。
     */
    public async Task ObserveEffectsAsync(
        ChannelReader<UiEffect> effectReader,
        Func<Func<Task>, Task> effectExecutor,
        Func<string, string, Task> showDialogAsync,
        CancellationToken cancellationToken = default)
    {
        await foreach (var effect in effectReader.ReadAllAsync(cancellationToken))
        {
            await effectExecutor(async () =>
            {
                await HandleEffectAsync(effect, showDialogAsync);
            });
        }
    }

    /**
     * One-shot Effect を解釈し、
     * 実際の UI 操作へ変換する。
     *
     * Navigation 系は本クラスで直接処理し、
     * Dialog 系は呼び出し側から渡された表示関数へ委譲する。
     */
    public async Task HandleEffectAsync(
        UiEffect effect,
        Func<string, string, Task> showDialogAsync)
    {
        switch (effect)
        {
            case NavigateToEffect navigate:
                // 指定ページへ遷移する
                _navigationService.Navigate(navigate.PageType, navigate.Parameter);
                break;

            case GoBackEffect:
                // 直前ページへ戻る
                _navigationService.GoBack();
                break;

            case ShowDialogEffect dialog:
                // ダイアログ表示を呼び出し側へ委譲する
                await showDialogAsync(dialog.Title, dialog.Message);
                break;

            case ShowToastEffect toast:
                // 現時点では簡易実装としてダイアログ表示へ寄せる
                await showDialogAsync("通知", toast.Message);
                break;

            default:
                throw new NotSupportedException(
                    $"Unsupported effect type: {effect.GetType().FullName}");
        }
    }
}