using IdIdentifyApp.Feature.Check.Ui.Intents;
using IdIdentifyApp.Feature.Check.Ui.ViewModels;
using IdIdentifyApp.Common.Ui.Locators;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace IdIdentifyApp.Feature.Check.Ui.Views;

/**
 * CheckPage1 の View。
 *
 * 動作確認用として、
 * State 更新による UI 変化と Effect による画面遷移を確認する。
 *
 * Effect の購読および実行責務は Shell 側へ委譲する。
 */
public sealed partial class CheckPage1 : Page
{
    private readonly CheckPage1ViewModel _viewModel;

    public CheckPage1()
    {
        InitializeComponent();

        _viewModel = ViewModelLocator.Resolve<CheckPage1ViewModel>();
        DataContext = _viewModel;
    }

     /**
     * 状態変更ボタン押下。
     *
     * 状態変更要求を Intent として ViewModel へ通知する。
     */
    private async void OnChangeStateClick(object sender, RoutedEventArgs e)
    {
        await _viewModel.PublishIntentAsync(new ChangeStateClicked());
    }

    /**
     * データ取得ボタン押下。
     *
     * UseCase 呼び出し要求を Intent として ViewModel へ通知する。
     */
    private async void OnLoadClick(object sender, RoutedEventArgs e)
    {
        await _viewModel.PublishIntentAsync(new LoadClicked());
    }

    /**
     * 画面遷移ボタン押下。
     *
     * Page2 への遷移要求を Intent として ViewModel へ通知する。
     */
    private async void OnNavigateClick(object sender, RoutedEventArgs e)
    {
        await _viewModel.PublishIntentAsync(new NavigateClicked());
    }
}