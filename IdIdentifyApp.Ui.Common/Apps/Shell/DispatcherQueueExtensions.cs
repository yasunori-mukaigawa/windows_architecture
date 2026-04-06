using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace IdIdentifyApp.Ui.Common.Apps.Shell;

/**
 * DispatcherQueue 用の共通拡張。
 *
 * 本クラスは、UI スレッドへの処理投入を
 * Task ベースで await 可能にする補助を提供する。
 *
 * ■ 提供する責務
 *   DispatcherQueue への非同期処理投入
 *   成功 / 失敗結果の Task 化
 *
 * ■ 設計上の意図
 *   Shell や Dialog 表示処理から
 *   DispatcherQueue の定型コードを分離する。
 */
public static class DispatcherQueueExtensions
{
    /**
     * 指定処理を DispatcherQueue へ投入し、
     * 完了を await 可能な Task として返す。
     */
    public static Task EnqueueAsync(this DispatcherQueue dispatcherQueue, Func<Task> action)
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
            tcs.SetException(
                new InvalidOperationException("Failed to enqueue action to DispatcherQueue."));
        }

        return tcs.Task;
    }
}