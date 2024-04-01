namespace BackendAdventureLeague.Models;

public class Request
{
    public long Id { get; set; }

    public DateTime ExpirationTime { get; set; }

    public decimal AmountToBuy { get; set; }
    
    public decimal CostToBy { get; set; }
    
    public Account AccountFrom { get; set; }
    
    public Account AccountTo { get; set; }

    public ApplicationUser User { get; set; }
}