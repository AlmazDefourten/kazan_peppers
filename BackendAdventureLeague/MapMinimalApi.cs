﻿using BackendAdventureLeague.Endpoints.Account;
using BackendAdventureLeague.Endpoints.Authorization;
using BackendAdventureLeague.Endpoints.History;
using BackendAdventureLeague.Endpoints.Prediction;
using BackendAdventureLeague.Endpoints.Request;
using BackendAdventureLeague.Models;
using BackendAdventureLeague.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAdventureLeague;

public class TransferDto
{
    public long IdFrom { get; set; }
    
    public long IdTo { get; set; }
    
    public decimal Sum { get; set; }
}

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
            .MapPost(async ([FromServices]IAccountCrudEndpoints service, TransferDto dto) => 
                                await service.TransferAsync(dto.IdFrom, dto.IdTo, dto.Sum), "transfer");
        
        app.MapGroup("request")
            .RequireAuthorization()
            .MapGet(async ([FromServices]IRequestService service) => await service.ListAsync(), "list")
            .MapGet(async ([FromServices]IRequestService service, long id) => await service.GetAsync(id), "get")
            .MapPost(async ([FromServices]IRequestService service, Request request) => await service.CreateAsync(request), "create")
            .MapDelete(async ([FromServices]IRequestService service, long id) => await service.DeleteAsync(id), "delete");

        app.MapGroup("operations")
            .RequireAuthorization()
            .MapGet(async ([FromServices] IOperationHistoryElementService service) => await service.ListOperations(), "list")
            .MapGet(async ([FromServices] IOperationHistoryElementService service) => await service.ListNeedToNotifyOperations(), "notifylist")
            .MapPost(
                async ([FromServices] IOperationHistoryElementService service, OperationHistoryElement element) =>
                await service.CreateAsync(element), "create");
        
        app.MapGroup("currency")
            .RequireAuthorization()
            .MapGet(() => CurrencyService.RoubleToYuanCourse.ToString() + " " + CurrencyService.RoubleToDyrhamCourse.ToString() + " " + CurrencyService.RoubleToDollar, "get");
        
        app.MapGroup("recomendation")
            .RequireAuthorization()
            .MapGet(() => BackgroundWorkerService.CachedRecommendations, "get");
        
        app.MapGroup("prediction")
            .RequireAuthorization()
            .MapGet(() => PredictionService.PredictionCalculated, "get");

        AuthorizationEndpoints.AddCustomAuthorizationEndpoints(app);
    }
}