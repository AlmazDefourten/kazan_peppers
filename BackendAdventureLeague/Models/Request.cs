namespace BackendAdventureLeague.Models;

public class Request
{
    public long Id { get; set; }

    public DateTime ExpirationTime { get; set; }

    public decimal AmountToBuy { get; set; }
    
    public CurrencyTypes CurrencyType { get; set; }
    
    public Account Account { get; set; }

    public ApplicationUser User { get; set; }
}