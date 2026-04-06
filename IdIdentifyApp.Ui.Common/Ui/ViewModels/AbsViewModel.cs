using IdIdentifyApp.Common.Domain.Error;
using IdIdentifyApp.Common.Ui.Intents;
using IdIdentifyApp.Common.Ui.Messages;
using IdIdentifyApp.Common.Ui.Mvi;
using IdIdentifyApp.Common.Ui.UiStates;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IdIdentifyApp.Common.Ui.ViewModels;

/**
 * MVI パターンに基づく共通 ViewModel 基底クラス。
 *
 * 本クラスは、UI状態管理と副作用制御を一貫したパターンで扱うための基盤を提供する。
 *
 * ■ 提供する責務
 *   UiState は immutable とし、Reducer 経由でのみ更新する
 *   UIイベントおよび UseCase 結果を Message として扱う
 *   ナビゲーションやダイアログ等の One-shot 操作を Effect として扱う
 *   Effect は Channel を用いて「一度だけ消費」されることを保証する
 *   View からの入力を Intent として受け付け、順次処理する
 *
 * ■ 設計上の制約（必ず守ること）
 *   Reducer 内では副作用を禁止する（純粋関数）
 *   State の直接変更は禁止（必ず Dispatch → Reduce を通す）
 *   Effect は State に保持してはならない
 *   View は ViewModel の具体メソッドを直接呼ばず、Intent を publish する
 *
 * ■ 補足
 *   Effect を Channel で扱うことで、再描画や再購読による二重実行を防ぐ。
 *   Intent 受付を本基底クラスへ集約することで、具象 ViewModel から基盤処理を排除する。
 */
public abstract class AbsViewModel<TState, TMessage, TIntent> :
    INotifyPropertyChanged,
    IDisposable,
    IHasEffectStream,
    IIntentPublisher
    where TState : BaseUiState
    where TMessage : BaseUiMessage
    where TIntent : BaseUiIntent
{
    // One-shot Effect を流すための Channel
    // Stateとは独立したストリームとして扱う
    private readonly Channel<UiEffect> _effectChannel;

    // View からの Intent を受け付ける Channel
    private readonly Channel<TIntent> _intentChannel;

    // Intent 処理ループの停止制御
    private readonly CancellationTokenSource _intentLoopCts;

    // Dispose済みフラグ
    private bool _disposed;

    // 現在の UI 状態（immutable前提）
    private TState _state;

    protected AbsViewModel(TState initialState)
    {
        _state = initialState;

        // Effectは「一度だけ消費される」ことが重要なため Channel を使用
        _effectChannel = Channel.CreateUnbounded<UiEffect>(new UnboundedChannelOptions
        {
            // 複数購読を許容（Shell / Viewなど）
            SingleReader = false,

            // ViewModel側は基本単一書き込みだが制約は設けない
            SingleWriter = false,

            // 同期継続を防ぐことで再入やデッドロックを回避
            AllowSynchronousContinuations = false
        });

        // Intentは順次処理したいため、Reader は単一とする
        _intentChannel = Channel.CreateUnbounded<TIntent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
            AllowSynchronousContinuations = false
        });

        _intentLoopCts = new CancellationTokenSource();

        // Intent 処理ループを基底クラスで開始する
        _ = ProcessIntentsAsync(_intentChannel.Reader, _intentLoopCts.Token);
    }

    /**
     * 復旧属性に応じたガイダンス文言を生成する。
     */
    public string BuildRecoveryGuidance(DomainError error)
    {
        if (error.RestartRequired)
        {
            return $"{error.UserMessage}\nアプリを再起動してください。";
        }

        if (error.Retryable)
        {
            return $"{error.UserMessage}\n再試行してください。";
        }

        if (error.Skippable)
        {
            return $"{error.UserMessage}\n必要に応じてスキップしてください。";
        }

        return error.UserMessage;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    /**
     * 現在の UI 状態。
     *
     * View はこの値にバインドされる。
     * 更新は必ず Reducer を経由して行う。
     */
    public TState State
    {
        get => _state;
        private set
        {
            // 同一参照の場合は更新しない（無駄な再描画防止）
            if (ReferenceEquals(_state, value))
            {
                return;
            }

            _state = value;

            // バインディング更新通知
            OnPropertyChanged();
        }
    }

    /**
     * One-shot Effect の購読用 Reader。
     *
     * View または Shell が購読し、
     * ナビゲーションやダイアログ等の UI 副作用を実行する。
     */
    public ChannelReader<UiEffect> EffectReader => _effectChannel.Reader;

    /**
     * View から Intent を受け付ける。
     *
     * View は具体メソッドを直接呼ばず、
     * 本メソッドを通じて入力イベントを publish する。
     */
    public ValueTask PublishIntentAsync(object intent, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        if (intent is not TIntent typedIntent)
        {
            throw new InvalidOperationException(
                $"Invalid intent type. Expected: {typeof(TIntent).FullName}, Actual: {intent.GetType().FullName}");
        }

        // Intent を内部 Channel へ投入
        return _intentChannel.Writer.WriteAsync(typedIntent, cancellationToken);
    }

    /**
     * Reducer（純粋関数）。
     *
     * 現在の State と Message を受け取り、
     * 次の State を生成して返す。
     *
     * 禁止事項:
     *   非同期処理
     *   IO処理
     *   Effect発火
     */
    protected abstract TState Reduce(TState currentState, TMessage message);

    /**
     * Intent の意味解釈を行う。
     *
     * 具象 ViewModel は本メソッドで
     * Intent → Message / UseCase / Effect の流れを記述する。
     */
    protected abstract Task HandleIntentAsync(TIntent intent, CancellationToken cancellationToken);

    /**
     * Message を投入し、State を更新する。
     *
     * ViewModel から Reducer を呼び出す唯一の入口。
     */
    protected void Dispatch(TMessage message)
    {
        ThrowIfDisposed();

        // Reducerで次状態を生成し、Stateを更新
        State = Reduce(State, message);
    }

    /**
     * One-shot Effect を発行する。
     *
     * ナビゲーション、ダイアログ、トースト等の
     * UI副作用を通知するために使用する。
     *
     * Effect は State に保持せず、
     * Channel を通じて一度だけ消費される。
     */
    protected ValueTask PublishEffectAsync(UiEffect effect, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        // Effect を Channel に流す
        return _effectChannel.Writer.WriteAsync(effect, cancellationToken);
    }

    /**
     * Intent を順次処理する共通ループ。
     *
     * View から受け取った Intent はすべてここを通り、
     * 具象 ViewModel の HandleIntentAsync へ委譲される。
     */
    private async Task ProcessIntentsAsync(ChannelReader<TIntent> intentReader, CancellationToken cancellationToken)
    {
        await foreach (var intent in intentReader.ReadAllAsync(cancellationToken))
        {
            await HandleIntentAsync(intent, cancellationToken);
        }
    }

    /**
     * Dispose 時の拡張ポイント。
     *
     * 派生クラスで購読解除やリソース解放を行う場合はここに記述する。
     */
    protected virtual void OnDispose()
    {
    }

    /**
     * PropertyChanged 通知。
     */
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /**
     * Dispose 済みチェック。
     */
    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    /**
     * リソース解放。
     *
     * Intent / Effect の各 Channel を完了状態にし、
     * 以降の書き込みを防止する。
     */
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        // Intent の発行を終了
        _intentChannel.Writer.TryComplete();

        // Intent 処理ループを停止
        _intentLoopCts.Cancel();
        _intentLoopCts.Dispose();

        // Effect の発行を終了
        _effectChannel.Writer.TryComplete();

        // 派生クラスのクリーンアップ
        OnDispose();

        GC.SuppressFinalize(this);
    }

}