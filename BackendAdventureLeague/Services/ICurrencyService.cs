using BackendAdventureLeague.Models;

namespace BackendAdventureLeague.Services;

public interface ICurrencyService
{
    decimal GetCurrency(CurrencyTypes curType);
    static decimal RoubleToYuanCourse { get; set; }
    static decimal RoubleToDyrhamCourse { get; set; }
}