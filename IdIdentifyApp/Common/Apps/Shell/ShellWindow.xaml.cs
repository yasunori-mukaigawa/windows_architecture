using IdIdentifyApp.Common.Apps.Navigation;
using IdIdentifyApp.Common.Ui.Mvi;
using IdIdentifyApp.Common.Ui.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Apps.Shell;

/**
 * アプリケーション全体の Shell となる Window。
 *
 * 本クラスは、アプリケーションのルート Window として
 * 画面遷移および One-shot Effect の購読と実行を担当する。
 */
public sealed partial class ShellWindow : Window
{
    private readonly INavigationService _navigationService;

    // 現在購読中の Effect を停止するための CancellationTokenSource
    private CancellationTokenSource? _effectObservationCts;

    public ShellWindow(INavigationService navigationService)
    {
        InitializeComponent();

        _navigationService = navigationService;

        // Shell が保持する RootFrame を NavigationService に渡す
        _navigationService.Initialize(RootFrame);

        // ページ遷移完了時に DataContext を見て Effect 購読を張り替える
        RootFrame.Navigated += OnFrameNavigated;

        // 初期表示ページ
        _navigationService.Navigate(typeof(IdIdentifyApp.Feature.Check.Ui.Views.CheckPage1));
    }

    /**
     * ページ遷移完了時に呼び出される。
     *
     * 現在表示中ページの DataContext が Effect ストリームを持つ場合、
     * Shell 側で購読を開始する。
     */
    private void OnFrameNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        // 以前の購読を停止
        _effectObservationCts?.Cancel();
        _effectObservationCts?.Dispose();

        _effectObservationCts = new CancellationTokenSource();

        if (RootFrame.Content is FrameworkElement element &&
            element.DataContext is IHasEffectStream effectSource)
        {
            _ = ObserveEffectsAsync(effectSource.EffectReader, _effectObservationCts.Token);
        }
    }

    /**
     * ViewModel の EffectReader を購読し、
     * Shell 側で One-shot Effect を順次解釈・実行する。
     */
    public Task ObserveEffectsAsync(ChannelReader<UiEffect> effectReader, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            await foreach (var effect in effectReader.ReadAllAsync(cancellationToken))
            {
                await DispatcherQueue.EnqueueAsync(async () =>
                {
                    await HandleEffectAsync(effect);
                });
            }
        }, cancellationToken);
    }

    /**
     * One-shot Effect を解釈し、実際の UI 動作へ変換する。
     */
    public async Task HandleEffectAsync(UiEffect effect)
    {
        switch (effect)
        {
            case NavigateToEffect navigate:
                // 指定ページへ遷移
                _navigationService.Navigate(navigate.PageType, navigate.Parameter);
                break;

            case GoBackEffect:
                // 前画面へ戻る
                _navigationService.GoBack();
                break;

            case ShowDialogEffect dialog:
                // ダイアログを表示
                await ShowDialogInternalAsync(dialog.Title, dialog.Message);
                break;

            case ShowToastEffect toast:
                // 現時点ではトースト未実装のためダイアログで代替
                await ShowDialogInternalAsync("通知", toast.Message);
                break;

            default:
                throw new NotSupportedException($"Unsupported effect type: {effect.GetType().FullName}");
        }
    }

    /**
     * ContentDialog を用いて簡易ダイアログを表示する。
     */
    private async Task ShowDialogInternalAsync(string title, string message)
    {
        if (Content is not FrameworkElement rootElement)
        {
            return;
        }

        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = rootElement.XamlRoot
        };

        await dialog.ShowAsync();
    }
}

internal static class DispatcherQueueExtensions
{
    public static Task EnqueueAsync(this Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue, Func<Task> action)
    {
        var tcs = new TaskCompletionSource<object?>();

        if (!dispatcherQueue.TryEnqueue(async () =>
        {
            try
            {
                await action();
                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }))
        {
            tcs.SetException(new InvalidOperationException("Failed to enqueue action to DispatcherQueue."));
        }

        return tcs.Task;
    }
}