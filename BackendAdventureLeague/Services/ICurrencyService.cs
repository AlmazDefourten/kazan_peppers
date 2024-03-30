using BackendAdventureLeague.Models;

namespace BackendAdventureLeague.Services;

public interface ICurrencyService
{
    decimal GetCurrency(CurrencyTypes curType);
    static decimal YuanCourse { get; set; }
    static decimal DyrhamCourse { get; set; }
}