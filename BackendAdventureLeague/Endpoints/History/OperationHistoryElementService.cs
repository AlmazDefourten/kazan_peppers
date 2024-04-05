using BackendAdventureLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Endpoints.History;

public class OperationHistoryElementService(IApplicationDbContext dbContext, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager) : IOperationHistoryElementService
{
    public async Task<IList<OperationHistoryElement>> ListOperations(CancellationToken token = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);
        
        return await dbContext.Operations
            .Where(ac => currentUser != null && ac.User.Id == currentUser.Id)
            .ToListAsync(cancellationToken: token);
    }
    
    public async Task<IList<OperationHistoryElement>> ListNeedToNotifyOperations(CancellationToken token = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);

        var result = await dbContext.Operations
            .Where(op => currentUser != null && op.User.Id == currentUser.Id)
            .Include(x => x.User)
            .Where(op => op.NeedToNotified)
            .ToListAsync(cancellationToken: token);

        foreach (var op in result)
        {
            op.NeedToNotified = false;
        }

        await dbContext.SaveChangesAsync(cancellationToken: token);

        return result;
    }
    
    public async Task CreateAsync(OperationHistoryElement element, CancellationToken cancellationToken = default)
    {
        await dbContext.Operations.AddAsync(element, cancellationToken);
    }
}