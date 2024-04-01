using System.ComponentModel;
using BackendAdventureLeague.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAdventureLeague.Services;

public class BackgroundWorkerService(IApplicationDbContext dbContext) : IBackgroundWorkerService
{
    public static BackgroundWorker BackgroundWorker = new BackgroundWorker();

    public void RequestWorker()
    {
        BackgroundWorker.DoWork += (sender, e) =>
        {
                while (true)
                {
                    var now = DateTime.Now;
                    var requests = dbContext.Requests
                        .Include(x => x.AccountTo)
                        .Include(x => x.AccountFrom)
                        .Include(x => x.User)
                        .Where(r => r.ExpirationTime.Year == now.Year && r.ExpirationTime.Month == now.Month
                                                        && r.ExpirationTime.Day == now.Day && r.ExpirationTime.Hour == now.Hour && r.ExpirationTime.Minute == now.Minute);
                    
                    foreach (var request in requests.ToList())
                    {
                        Console.WriteLine(request.AmountToBuy);
                        var from = request.AccountFrom;
                        var to = request.AccountTo;

                        decimal toSum = 0;
                        decimal toMinus = 0;

                        switch (from.CurrencyType)
                        {
                            case(CurrencyTypes.Ruble):
                                switch (to.CurrencyType)
                                {
                                    case CurrencyTypes.Ruble:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy;
                                        break;
                                    case CurrencyTypes.Dirham:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy * CurrencyService.RoubleToDyrhamCourse;
                                        break;
                                    case CurrencyTypes.Yuan:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy * CurrencyService.RoubleToYuanCourse;
                                        break;
                                }
                                break;
                            case CurrencyTypes.Yuan:
                                switch (to.CurrencyType)
                                {
                                    case CurrencyTypes.Yuan:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy;
                                        break;
                                    case CurrencyTypes.Dirham:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy * CurrencyService.YuanToDyrhamCourse;
                                        break;
                                    case CurrencyTypes.Ruble:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy * CurrencyService.YuanToRoubleCourse;
                                        break;
                                }
                                break;
                            case CurrencyTypes.Dirham:
                                switch (to.CurrencyType)
                                {
                                    case CurrencyTypes.Dirham:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy;
                                        break;
                                    case CurrencyTypes.Ruble:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy * CurrencyService.DyrhamToRoubleCourse;
                                        break;
                                    case CurrencyTypes.Yuan:
                                        toSum = request.AmountToBuy;
                                        toMinus = request.AmountToBuy * CurrencyService.DyrhamToYuanCourse;
                                        break;
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (from.Sum < request.AmountToBuy || from.CurrencyType == to.CurrencyType)
                        {
                            return;
                        }

                        if (from.Sum - toMinus < 0)
                        {
                            return;
                        }
                        
                        to.Sum += Math.Round(toSum, 2);
                        from.Sum -= Math.Round(toMinus, 2);
        
                        dbContext.SaveChanges();
                    }
                    
                    // Подождите 1 минуту перед следующей итерацией
                    Thread.Sleep(55000); // 60000 миллисекунд = 1 минута
                }
        };

        BackgroundWorker.RunWorkerAsync();
    }
}

public interface IBackgroundWorkerService
{
    void RequestWorker();
}