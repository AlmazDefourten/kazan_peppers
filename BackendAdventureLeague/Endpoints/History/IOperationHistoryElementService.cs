using BackendAdventureLeague.Models;

namespace BackendAdventureLeague.Endpoints.History;

public interface IOperationHistoryElementService
{
    Task<IList<OperationHistoryElement>> ListOperations(CancellationToken token = default);

    Task<IList<OperationHistoryElement>> ListNeedToNotifyOperations(CancellationToken token = default);
    
    Task CreateAsync(OperationHistoryElement element, CancellationToken cancellationToken = default);
}