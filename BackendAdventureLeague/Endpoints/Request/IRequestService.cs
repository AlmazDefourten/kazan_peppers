namespace BackendAdventureLeague.Endpoints.Request;

public interface IRequestService
{
    Task CreateAsync(Models.Request account, CancellationToken cancellationToken = default);
    
    Task<IList<Models.Request>> ListAsync(CancellationToken cancellationToken = default);
    
    Task<Models.Request?> GetAsync(long id, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}