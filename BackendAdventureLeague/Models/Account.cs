namespace BackendAdventureLeague.Models;

public class Account
{
    public long Id { get; set; }
    
    public decimal Sum { get; set; }

    public string Name { get; set; } = "";

    public CurrencyTypes CurrencyType { get; set; }
    public ApplicationUser User { get; set; }
}