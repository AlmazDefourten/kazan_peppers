using BackendAdventureLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseNpgsql("Server=localhost;Port=5432;Database=backend;User Id=postgres;Password=123654gg;");
});

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Tokens.ProviderMap.Add("Phone", new TokenProviderDescriptor(typeof(PhoneNumberTokenProvider<ApplicationUser>)));
    options.Tokens.ChangePhoneNumberTokenProvider = "Phone";
});

builder.Services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder();


builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

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
    .WithOpenApi();

app.MapIdentityApi<ApplicationUser>();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
await context?.Database.MigrateAsync()!;

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}