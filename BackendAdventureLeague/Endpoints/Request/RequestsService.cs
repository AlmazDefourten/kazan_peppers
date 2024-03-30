using BackendAdventureLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Endpoints.Request;

public class RequestsService(IApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager) : IRequestService
{
    public async Task CreateAsync(Models.Request account, CancellationToken cancellationToken = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);

        var gettedUser = await context.Users.FindAsync(currentUser.Id);

        if (gettedUser != null) account.User = gettedUser;
        await context.Requests.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<Models.Request>> ListAsync(CancellationToken cancellationToken = default)
    {
        var claims = contextAccessor.HttpContext?.User;
        var currentUser = await userManager.GetUserAsync(claims!);

        return await context.Requests
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