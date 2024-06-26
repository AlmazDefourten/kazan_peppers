using BackendAdventureLeague;
using BackendAdventureLeague.Endpoints.Account;
using BackendAdventureLeague.Endpoints.History;
using BackendAdventureLeague.Endpoints.Prediction;
using BackendAdventureLeague.Endpoints.Request;
using BackendAdventureLeague.Models;
using BackendAdventureLeague.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
{
    Exception ex = (Exception)args.ExceptionObject;
    Console.WriteLine("Произошло необработанное исключение: " + ex.Message);
};

var currencyServiceInit = new CurrencyService();
currencyServiceInit.GetCurrency(CurrencyTypes.Yuan);
var predictionServiceInit = new PredictionService();
predictionServiceInit.GeneratePredictions();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseNpgsql("Server=31.129.111.35;Port=5432;Database=backend;User Id=postgres;Password=123654gg;Include Error Detail=true;");
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
builder.Services.AddScoped<IBackgroundWorkerService, BackgroundWorkerService>();

builder.Services.AddSingleton<ICurrencyService, CurrencyService>();

builder.Services.AddTransient<IAccountCrudEndpoints, AccountCrudEndpoints>();
builder.Services.AddTransient<IRequestService, RequestsService>();

builder.Services.AddScoped<IPredictionService, PredictionService>();
builder.Services.AddScoped<IOperationHistoryElementService, OperationHistoryElementService>();

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

MapMinimalApi.DoMap(app);

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
await context?.Database.MigrateAsync()!;

var worker = scope.ServiceProvider.GetRequiredService<IBackgroundWorkerService>();
worker.RequestWorker();

app.Run();

