using BackendAdventureLeague.Models;
using BackendAdventureLeague.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Endpoints.Account;

public class AccountCrudEndpoints(IApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, ICurrencyService currencyService) : IAccountCrudEndpoints
{
    public async Task CreateAsync(Models.Account account, CancellationToken cancellationToken = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);

        if (currentUser != null) account.User = currentUser;
        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Models.Account>> ListAsync(CancellationToken cancellationToken = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);

        return await context.Accounts
            .Where(ac => currentUser != null && ac.User.Id == currentUser.Id)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Models.Account?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await context.Accounts.FindAsync(id);
    }
    
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        context.Accounts.Remove(await context.Accounts.FindAsync(id));
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task TransferAsync(long idFrom, long idTo, decimal sum, CancellationToken cancellationToken = default)
    {
        var from = await context.Accounts.FindAsync(idFrom);
        var to = await context.Accounts.FindAsync(idTo);

        currencyService.TransferMoney(from, to, sum);
        
        await context.SaveChangesAsync(cancellationToken);
    }
}