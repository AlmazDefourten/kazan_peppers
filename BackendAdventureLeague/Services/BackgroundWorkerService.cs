using System.ComponentModel;
using BackendAdventureLeague.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Services;

public class BackgroundWorkerService(IApplicationDbContext dbContext, ICurrencyService currencyService) : IBackgroundWorkerService
{
    private static readonly BackgroundWorker BackgroundWorker = new();

    public void RequestWorker()
    {
        BackgroundWorker.DoWork += (_, _) =>
        {
                while (true)
                {
                    var now = DateTime.Now;
                    
                    if (now is { Hour: 11, Minute: 31 })
                    {
                        currencyService.GetCurrency(CurrencyTypes.Dirham);
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
                        
                        dbContext.SaveChanges();
                    }
                    
                    var requestsToDelete = dbContext.Requests
                        .Include(x => x.AccountTo)
                        .Include(x => x.AccountFrom)
                        .Include(x => x.User)
                        .Where(r => r.ExpirationTime.Year <= now.Year && r.ExpirationTime.Month <= now.Month
                                                                      && r.ExpirationTime.Day <= now.Day && r.ExpirationTime.Hour <= now.Hour && r.ExpirationTime.Minute < now.Minute);
                    
                    foreach (var request in requestsToDelete)
                    {
                        request.IsActual = false;
                    }

                    dbContext.SaveChanges();
                    
                    // Подождите 1 минуту перед следующей итерацией
                    Thread.Sleep(60000); // 60000 миллисекунд = 1 минута
                }
        };
        
        BackgroundWorker.RunWorkerAsync();
    }
}

public interface IBackgroundWorkerService
{
    void RequestWorker();
}