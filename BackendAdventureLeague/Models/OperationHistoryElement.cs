namespace BackendAdventureLeague.Models;

public class OperationHistoryElement
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime TimeMoment { get; set; }
    
    public bool NeedToNotified { get; set; }
    
    public ApplicationUser User { get; set; }
    
    public Request Request { get; set; }
}