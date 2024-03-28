namespace BackendAdventureLeague.Endpoints.Account;

public interface IAccountCrudEndpoints
{
    Task CreateAsync(Models.Account account, CancellationToken cancellationToken = default);

    Task<IList<Models.Account>> ListAsync(CancellationToken cancellationToken = default);

    Task<Models.Account?> GetAsync(long id, CancellationToken cancellationToken = default);

    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}