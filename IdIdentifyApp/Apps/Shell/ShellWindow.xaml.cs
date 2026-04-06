using IdIdentifyApp.Ui.Common.Apps.Navigation;
using IdIdentifyApp.Ui.Common.Apps.Shell;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Apps.Shell;

/**
 * アプリケーション全体の Shell となる Window。
 *
 * 本クラスは、アプリケーションのルート Window として
 * 画面全体の組み立てを担当する。
 *
 * ■ 提供する責務
 *   RootFrame を保持する
 *   Shell 共通コーディネータを初期化する
 *   ページ遷移後の Effect 購読切り替えを行う
 *   アプリ固有の初期画面遷移を決定する
 *   ContentDialog を実表示する
 *
 * ■ 設計上の意図
 *   Navigation / Effect 解釈の共通ロジックは
 *   Ui.Common 側へ分離し、
 *   本クラスにはアプリ固有の構成責務だけを残す。
 */
public partial class ShellWindow : Window
{
    private readonly INavigationService _navigationService;
    private readonly ShellEffectCoordinator _shellEffectCoordinator;

    // 現在購読中の Effect を停止するための CancellationTokenSource
    private CancellationTokenSource? _effectObservationCts;

    public ShellWindow(
        INavigationService navigationService,
        ShellEffectCoordinator shellEffectCoordinator)
    {
        InitializeComponent();

        _navigationService = navigationService;
        _shellEffectCoordinator = shellEffectCoordinator;

        // Shell が保持する RootFrame を NavigationService に接続する
        _shellEffectCoordinator.Initialize(RootFrame);

        // ページ遷移後に Effect 購読先を張り替える
        RootFrame.Navigated += OnFrameNavigated;

        // アプリ起動時の初期画面へ遷移する
        NavigateToInitialPage();
    }

    /**
     * アプリ起動時の初期画面へ遷移する。
     *
     * 初期ページの決定はアプリ固有の責務として本クラスに残す。
     */
    private void NavigateToInitialPage()
    {
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
        // 以前の購読を停止する
        _effectObservationCts?.Cancel();
        _effectObservationCts?.Dispose();

        _effectObservationCts = new CancellationTokenSource();

        var effectSource = _shellEffectCoordinator.FindEffectSource(RootFrame.Content);
        if (effectSource is null)
        {
            return;
        }

        _ = _shellEffectCoordinator.ObserveEffectsAsync(
            effectSource.EffectReader,
            ExecuteOnUiThreadAsync,
            ShowDialogInternalAsync,
            _effectObservationCts.Token);
    }

    /**
     * 指定処理を UI スレッド上で実行する。
     *
     * ShellEffectCoordinator からの共通処理委譲先として使用する。
     */
    private Task ExecuteOnUiThreadAsync(Func<Task> action)
    {
        return DispatcherQueue.EnqueueAsync(action);
    }

    /**
     * ContentDialog を用いて簡易ダイアログを表示する。
     *
     * 実際の表示方法はアプリ側責務として本クラスに残す。
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