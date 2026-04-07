using IdIdentifyApp.Applications.Feature.Check.Intents;
using IdIdentifyApp.Feature.Check.Ui.ViewModels;
using IdIdentifyApp.Ui.Common.Apps.Locators;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading;

namespace IdIdentifyApp.Feature.Check.Ui.Views;

/**
 * CheckPage2 の View。
 *
 * 動作確認用として、
 * State 更新による UI 変化と GoBackEffect による戻る操作を確認する。
 */
public sealed partial class CheckPage2 : Page
{
    private readonly CheckPage2ViewModel _viewModel;
    private CancellationTokenSource? _effectCts;
    private bool _effectObservationStarted;

    public CheckPage2()
    {
        InitializeComponent();

        _viewModel = ViewModelLocator.Resolve<CheckPage2ViewModel>();
        DataContext = _viewModel;
    }

    private async void OnCompleteClick(object sender, RoutedEventArgs e)
    {
        await _viewModel.PublishIntentAsync(new CompleteClicked());
    }

    private async void OnBackClick(object sender, RoutedEventArgs e)
    {
        await _viewModel.PublishIntentAsync(new BackClicked());
    }
}