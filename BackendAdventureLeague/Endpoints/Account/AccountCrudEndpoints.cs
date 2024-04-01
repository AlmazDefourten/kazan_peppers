using BackendAdventureLeague.Models;
using BackendAdventureLeague.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Endpoints.Account;

public class AccountCrudEndpoints(IApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager) : IAccountCrudEndpoints
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

        decimal toSum = 0;
        decimal toMinus = 0;

        switch (from.CurrencyType)
        {
            case(CurrencyTypes.Ruble):
                switch (to.CurrencyType)
                {
                    case CurrencyTypes.Ruble:
                        toSum = sum;
                        toMinus = sum;
                        break;
                    case CurrencyTypes.Dirham:
                        toSum = sum;
                        toMinus = sum * CurrencyService.RoubleToDyrhamCourse;
                        break;
                    case CurrencyTypes.Yuan:
                        toSum = sum;
                        toMinus = sum * CurrencyService.RoubleToYuanCourse;
                        break;
                }
                break;
            case CurrencyTypes.Yuan:
                switch (to.CurrencyType)
                {
                    case CurrencyTypes.Yuan:
                        toSum = sum;
                        toMinus = sum;
                        break;
                    case CurrencyTypes.Dirham:
                        toSum = sum;
                        toMinus = sum * CurrencyService.YuanToDyrhamCourse;
                        break;
                    case CurrencyTypes.Ruble:
                        toSum = sum;
                        toMinus = sum * CurrencyService.YuanToRoubleCourse;
                        break;
                }
                break;
            case CurrencyTypes.Dirham:
                switch (to.CurrencyType)
                {
                    case CurrencyTypes.Dirham:
                        toSum = sum;
                        toMinus = sum;
                        break;
                    case CurrencyTypes.Ruble:
                        toSum = sum;
                        toMinus = sum * CurrencyService.DyrhamToRoubleCourse;
                        break;
                    case CurrencyTypes.Yuan:
                        toSum = sum;
                        toMinus = sum * CurrencyService.DyrhamToYuanCourse;
                        break;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (from.Sum < sum || from.CurrencyType == to.CurrencyType)
        {
            return;
        }

        to.Sum += Math.Round(toSum, 2);
        from.Sum -= Math.Round(toMinus, 2);
        
        await context.SaveChangesAsync(cancellationToken);
    }
}