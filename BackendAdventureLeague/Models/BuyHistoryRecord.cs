namespace BackendAdventureLeague.Models;

public class BuyHistoryRecord
{
    public long Id { get; set; }
    
    public CurrencyTypes CurrencyType { get; set; }
    
    public decimal Sum { get; set; }
    
    public ApplicationUser User { get; set; }
}