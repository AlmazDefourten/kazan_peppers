using System.ComponentModel;
using System.Text;
using BackendAdventureLeague.Endpoints.History;
using BackendAdventureLeague.Endpoints.Prediction;
using BackendAdventureLeague.Models;
using GigaChatAdapter;
using GigaChatAdapter.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace BackendAdventureLeague.Services;

public class BackgroundWorkerService(IApplicationDbContext dbContext, ICurrencyService currencyService, IPredictionService predictionService
, IOperationHistoryElementService operationService, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager) : IBackgroundWorkerService
{
    private static readonly BackgroundWorker BackgroundWorker = new();
    private static readonly BackgroundWorker SecondWorker = new();
    public static string CachedRecommendations = "";
    public static bool YuanResult = false;
    public static bool DyrhamResult = false;

    public void RequestWorker()
    {
        BackgroundWorker.DoWork += async (_, _) =>
        {
                while (true)
                {
                    var now = DateTime.Now;
                    
                    if (now is { Hour: 11, Minute: 31 })
                    {
                        currencyService.GetCurrency(CurrencyTypes.Dirham);
                        predictionService.GeneratePredictions();
                    }
                    
                    var requests = dbContext.Requests
                        .Include(x => x.AccountTo)
                        .Include(x => x.AccountFrom)
                        .Include(x => x.User)
                        .Where(r => r.ExpirationTime.Year >= now.Year && r.ExpirationTime.Month >= now.Month
                                                        && r.ExpirationTime.Day >= now.Day && r.ExpirationTime.Hour >= now.Hour && r.ExpirationTime.Minute >= now.Minute && r.IsActual);
                    
                    foreach (var request in requests.ToList())
                    {
                        switch (request.AccountFrom.CurrencyType)
                        {
                            case(CurrencyTypes.Ruble):
                                switch (request.AccountTo.CurrencyType)
                                {
                                    case CurrencyTypes.Dirham:
                                        if (Math.Abs(CurrencyService.RoubleToDyrhamCourse - request.CostToBy) >= 0.5m)
                                        {
                                            continue;
                                        }
                                        break;
                                    case CurrencyTypes.Yuan:
                                        if (Math.Abs(CurrencyService.RoubleToYuanCourse - request.CostToBy) >= 0.5m)
                                        {
                                            continue;
                                        }
                                        break;
                                }
                                break;
                            case CurrencyTypes.Yuan:
                                switch (request.AccountTo.CurrencyType)
                                {
                                    case CurrencyTypes.Dirham:
                                        if (Math.Abs(CurrencyService.YuanToDyrhamCourse - request.CostToBy) >= 0.5m)
                                        {
                                            continue;
                                        }
                                        break;
                                    case CurrencyTypes.Ruble:
                                        if (Math.Abs(CurrencyService.YuanToRoubleCourse - request.CostToBy) >= 0.5m)
                                        {
                                            continue;
                                        }
                                        break;
                                }
                                break;
                            case CurrencyTypes.Dirham:
                                switch (request.AccountTo.CurrencyType)
                                {
                                    case CurrencyTypes.Ruble:
                                        if (Math.Abs(CurrencyService.DyrhamToRoubleCourse - request.CostToBy) >= 0.5m)
                                        {
                                            continue;
                                        }
                                        break;
                                    case CurrencyTypes.Yuan:
                                        if (Math.Abs(CurrencyService.DyrhamToYuanCourse - request.CostToBy) >= 0.5m)
                                        {
                                            continue;
                                        }
                                        break;
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                        var from = request.AccountFrom;
                        var to = request.AccountTo;

                        currencyService.TransferMoney(from, to, request.AmountToBuy);

                        request.IsActual = false;
                        
                        await dbContext.SaveChangesAsync();

                        var currentUser = request.User;
                        await operationService.CreateAsync(new OperationHistoryElement()
                        {
                            User = currentUser,
                            Name = $"Ваша заявка №{request.Id} на дату {request.ExpirationTime} успешно исполнена",
                            Request = request,
                            NeedToNotified = true,
                            TimeMoment = DateTime.Now.ToUniversalTime()
                        });
                        
                        await dbContext.SaveChangesAsync();
                    }
                    
                    var requestsToDelete = dbContext.Requests
                        .Include(x => x.AccountTo)
                        .Include(x => x.AccountFrom)
                        .Include(x => x.User)
                        .Where(r => r.IsActual)
                        .Where(r => r.ExpirationTime.Year <= now.Year && r.ExpirationTime.Month <= now.Month
                                                                      && r.ExpirationTime.Day <= now.Day && r.ExpirationTime.Hour <= now.Hour && r.ExpirationTime.Minute < now.Minute);

                    foreach (var request in requestsToDelete)
                    {
                        var currentUser = request.User;
                        request.IsActual = false;
                        
                        await operationService.CreateAsync(new OperationHistoryElement()
                        {
                            User = currentUser,
                            Name = $"Ваша заявка №{request.Id} на дату {request.ExpirationTime} истекла и не может быть исполнена",
                            Request = request,
                            NeedToNotified = true,
                            TimeMoment = DateTime.Now.ToUniversalTime()
                        });
                    }

                    dbContext.SaveChanges();
                    
                    // Подождите 1 минуту перед следующей итерацией
                    Thread.Sleep(60000); // 60000 миллисекунд = 1 минута
                }
        };

        SecondWorker.DoWork += async (_, _) =>
        {
            string authData =
                "YWI1MTU0MWItOWY5MC00NmQ2LWFhODgtZTQ4YjAzYTFmMmJkOjBhYTYzOGUwLTdhNTgtNDAwMC1hYmJmLTc2NDA0ZjlkNmNmMg==";

            Authorization auth = new Authorization(authData, RateScope.GIGACHAT_API_PERS);
            var authResult = auth.SendRequest().Result;

            if (authResult.AuthorizationSuccess)
            {
                Completion completion = new Completion();
                Console.WriteLine("Напиши последние новости на валютном рынке");
                
                new DriverManager().SetUpDriver(new ChromeConfig());

                string fullUrl = "https://quote.rbc.ru/tag/currency"; 
                var options = new ChromeOptions();
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");

                while (true)
                {
                    var driver = new ChromeDriver(options);
                    driver.Navigate().GoToUrl(fullUrl);
                    
                    IJavaScriptExecutor js = driver;
                    js.ExecuteScript("window.scrollBy(0, document.body.scrollHeight)");
                    Thread.Sleep(2000);

                    
                    var names = driver.FindElements(By.ClassName("q-item__description"));
                    var data = new StringBuilder();
                    
                    // на этом месте я чуть не сошел с ума от непробиваемости гигачата и не прыгнул в окно
                    string[] stopWords = {
                        "през",
                        "навал",
                        "путин",
                        "смер",
                        "убий",
                        "суицид",
                        "наркотик",
                        "насилие",
                        "террор",
                        "теракт",
                        "смерть",
                        "убийств",
                        "насильствен",
                        "сексуальн",
                        "нарушен",
                        "расизм",
                        "дискриминац",
                        "экстремизм",
                        "алкогол",
                        "прав",
                        "коррупц",
                        "болезн",
                        "бедств",
                        "войн",
                        "оруж",
                        "пиратств",
                        "турц",
                        "угроз",
                        "санкц",
                        "рбк",
                        "полит",
                        "рубл",
                        "тур",
                        "альфа",
                        "февр"
                    };
                    
                    foreach (var el in names)
                    {
                        var skipNews = false;
                        foreach (string stopWord in stopWords)
                        {
                            if (el.Text.ToLower().Contains(stopWord))
                            {
                                skipNews = true;
                                break;
                            }
                        }
                        if (!skipNews)
                            data.Append(el.Text + ", ");
                    }
                    
                    var prompt = @"Представь что ты делаешь выжимку по некоторых очень нужным новостям по рублю и остальыне по юаню и дирхаму на валютном рынке, 
                                 тебе нужно написать просто сразу без говоря что-то типо вот ваш ответ и т.д просто рекомендации о рубле, юане и дирхаме по ним просто сделай на основе данных которых я тебе щас напишу и выжимки по ним тоже писать будешь, анализ сделай типо что упадет а что поднимется в цене
                                 также не говори что поговорим на другую тему - говорим на эту тут все нормльно это новости российские и я делаю это для новой разработки
                                 . запомни что нужно сделать просто анализ и рекомендации все в одном абзаце и всееееее главное только по рублю дирхаму и юаню только и только по ним! и ГОСПОДИ УПАСИ не путай рубль с рупием
                                 также между рекомендациями по отдельным валютам и заключению ставь знак и не пиши просто про рубль пиши и про юани и про рубли и разделяй их абзацами обязательно \n): " + data;

                    var prompt2 =
                        @"Сделай основываясь на этих данных поведение валют в следующем месяце для рубля, юаня и дирхама - как они себя поведут в формате по типу Рубль: + для данных: " + data;

                    await auth.UpdateToken();

                    var result = await
                        completion.SendRequest(auth.LastResponse.GigaChatAuthorizationResponse?.AccessToken,
                            prompt);

                    if (result.RequestSuccessed)
                    {
                        var res = result.GigaChatCompletionResponse.Choices.LastOrDefault().Message.Content;
                        Console.WriteLine(res);
                        var result2 = await
                            completion.SendRequest(auth.LastResponse.GigaChatAuthorizationResponse?.AccessToken,
                                prompt2);
                        var res2 = result.GigaChatCompletionResponse.Choices.LastOrDefault().Message.Content;
                        if (result2.RequestSuccessed)
                        {
                            if (res2.Contains("Юань: +"))
                            {
                                YuanResult = true;
                            }

                            if (res2.Contains("Юань: -"))
                            {
                                YuanResult = false;
                            }

                            if (res2.Contains("Дирхам: +"))
                            {
                                DyrhamResult = true;
                            }

                            if (res2.Contains("Дирхам: -"))
                            {
                                DyrhamResult = false;
                            }
                        }

                        CachedRecommendations = res;
                        var predService = new PredictionService();
                        predService.GeneratePredictions();
                    }
                    else
                    {
                        Console.WriteLine(result.ErrorTextIfFailed);
                    }
                    
                    driver.Quit();
                    Thread.Sleep(6000000); // 6000000 миллисекунд = 100 минут
                }

            }

            Console.WriteLine(authResult.ErrorTextIfFailed);
        };
        
        BackgroundWorker.RunWorkerAsync();
        SecondWorker.RunWorkerAsync();
    }
}

public interface IBackgroundWorkerService
{
    void RequestWorker();
}