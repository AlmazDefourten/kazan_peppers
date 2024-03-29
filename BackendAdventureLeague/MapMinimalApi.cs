using BackendAdventureLeague.Endpoints.Account;
using BackendAdventureLeague.Endpoints.Authorization;
using BackendAdventureLeague.Endpoints.Request;
using BackendAdventureLeague.Models;
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
            .MapGet(async ([FromServices]IAccountCrudEndpoints service) => await service.ListAsync(), "list");
        
        app.MapGroup("request")
            .RequireAuthorization()
            .MapGet(async ([FromServices]IRequestService service, long id) => await service.GetAsync(id), "get")
            .MapPost(async ([FromServices]IRequestService service, Request request) => await service.CreateAsync(request), "create")
            .MapDelete(async ([FromServices]IRequestService service, long id) => await service.DeleteAsync(id), "delete")
            .MapGet(async ([FromServices]IRequestService service) => await service.ListAsync(), "list");
        

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", () =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        (
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 0),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .RequireAuthorization()
            .WithOpenApi();

        AuthorizationEndpoints.AddCustomAuthorizationEndpoints(app);
    }
}