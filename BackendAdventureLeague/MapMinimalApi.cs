using BackendAdventureLeague.Endpoints.Account;
using BackendAdventureLeague.Endpoints.Authorization;
using BackendAdventureLeague.Endpoints.Request;
using BackendAdventureLeague.Models;
using BackendAdventureLeague.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAdventureLeague;

public static class MapMinimalApi
{
    public static void DoMap(WebApplication app)
    {
        app.MapGroup("account")
            .RequireAuthorization()
            .MapGet(async ([FromServices]IAccountCrudEndpoints service, long id) => await service.GetAsync(id), "get")
            .MapPost(async ([FromServices]IAccountCrudEndpoints service, Account account) => await service.CreateAsync(account), "create")
            .MapDelete(async ([FromServices]IAccountCrudEndpoints service, long id) => await service.DeleteAsync(id), "delete")
            .MapGet(async ([FromServices]IAccountCrudEndpoints service) => await service.ListAsync(), "list")
            .MapPost(async ([FromServices]IAccountCrudEndpoints service, long idFrom, long idTo, decimal sum) => 
                                await service.TransferAsync(idFrom, idTo, sum), "transfer");
        
        app.MapGroup("request")
            .RequireAuthorization()
            .MapGet(async ([FromServices]IRequestService service, long id) => await service.GetAsync(id), "get")
            .MapPost(async ([FromServices]IRequestService service, Request request) => await service.CreateAsync(request), "create")
            .MapDelete(async ([FromServices]IRequestService service, long id) => await service.DeleteAsync(id), "delete");
        
        app.MapGroup("currency")
            .RequireAuthorization()
            .MapGet(() => CurrencyService.YuanCourse.ToString() + " " + CurrencyService.DyrhamCourse.ToString(), "get");

        AuthorizationEndpoints.AddCustomAuthorizationEndpoints(app);
    }
}