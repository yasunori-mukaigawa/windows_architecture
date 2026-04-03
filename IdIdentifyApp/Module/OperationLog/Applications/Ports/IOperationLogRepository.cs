using IdIdentifyApp.Modules.OperationLog.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace IdIdentifyApp.Modules.OperationLog.Application.Ports;

/**
 * 操作ログ永続化の契約。
 *
 * 本契約は、操作ログ Entity を永続化する責務を持つ。
 *
 * ■ 設計上の意図
 *   UseCase やログサービスが具象 Repository 実装へ直接依存しないようにし、
 *   永続化手段の差し替えを可能にする。
 */
public interface IOperationLogRepository
{
    /**
     * 操作ログを保存する。
     */
    Task SaveAsync(OperationLogEntity entity, CancellationToken cancellationToken = default);
}