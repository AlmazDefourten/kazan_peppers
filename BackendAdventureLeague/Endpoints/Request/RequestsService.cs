using BackendAdventureLeague.Endpoints.History;
using BackendAdventureLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Endpoints.Request;

public class RequestsService(IApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, 
    IOperationHistoryElementService operationService) : IRequestService
{
    public async Task CreateAsync(Models.Request request, CancellationToken cancellationToken = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);
        context.Accounts.Entry(request.AccountFrom).State = EntityState.Unchanged;
        context.Accounts.Entry(request.AccountTo).State = EntityState.Unchanged;

        if (currentUser != null) request.User = currentUser;
        await context.Requests.AddAsync(request, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Models.Request>> ListAsync(CancellationToken cancellationToken = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);

        return await context.Requests
            .Include(req => req.AccountFrom)
            .Include(req => req.AccountTo)
            .Where(ac => currentUser != null && ac.User.Id == currentUser.Id)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Models.Request?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        return await context.Requests.FindAsync(id, cancellationToken);
    }
    
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        context.Requests.Remove(await context.Requests.FindAsync(id));
        await context.SaveChangesAsync(cancellationToken);
    }
}