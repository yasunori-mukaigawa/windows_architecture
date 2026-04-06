using Microsoft.UI.Xaml.Controls;
using System;

namespace IdIdentifyApp.Ui.Common.Apps.Navigation;

/**
 * 画面遷移を抽象化するサービス契約。
 *
 * 本インターフェースは、Shell が保持する Frame を用いた
 * 画面遷移操作を抽象化するために定義する。
 *
 * ■ 提供する責務
 *   Frame の初期化
 *   指定ページへの遷移
 *   戻る操作の可否判定
 *   戻る操作の実行
 *
 * ■ 設計上の意図
 *   ViewModel から直接 Frame を扱わせず、
 *   ナビゲーションの実行責務を App / Shell 側へ閉じ込める。
 */
public interface INavigationService
{
    /**
     * 画面遷移に使用する Frame を初期化する。
     */
    void Initialize(Frame frame);

    /**
     * 指定したページへ遷移する。
     */
    bool Navigate(Type pageType, object parameter = null);

    /**
     * 戻る操作が可能かを返す。
     */
    bool CanGoBack { get; }

    /**
     * 1つ前の画面へ戻る。
     */
    void GoBack();
}

/**
 * WinUI3 の Frame を用いた画面遷移サービス実装。
 *
 * 本クラスは、Shell が保持する Frame を内部に保持し、
 * アプリケーション全体のナビゲーション操作を実行する。
 *
 * ■ 設計上の制約
 *   Initialize 前に Navigate / GoBack を呼び出してはならない
 *   ViewModel から直接使用するのではなく、
 *   Effect を解釈する Shell 側から使用する
 */
public sealed class NavigationService : INavigationService
{
    // 実際の画面遷移に使用する Frame
    private Frame _frame;

    /**
     * 画面遷移に使用する Frame を初期化する。
     *
     * ShellWindow 作成時に一度だけ呼び出すことを想定する。
     */
    public void Initialize(Frame frame)
    {
        _frame = frame ?? throw new ArgumentNullException(nameof(frame));
    }

    /**
     * 指定したページへ遷移する。
     *
     * Frame が未初期化の場合は使用誤りとして例外を送出する。
     */
    public bool Navigate(Type pageType, object parameter = null)
    {
        if (_frame is null)
        {
            throw new InvalidOperationException("NavigationService is not initialized.");
        }

        // Frame に対してページ遷移を実行
        return _frame.Navigate(pageType, parameter);
    }

    /**
     * 戻る操作が可能かを返す。
     */
    public bool CanGoBack => _frame?.CanGoBack ?? false;

    /**
     * 1つ前の画面へ戻る。
     *
     * 戻る操作が不可能な場合は何もしない。
     */
    public void GoBack()
    {
        if (_frame?.CanGoBack == true)
        {
            // Frame の戻る処理を実行
            _frame.GoBack();
        }
    }
}