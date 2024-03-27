using Microsoft.AspNetCore.Identity;

namespace BackendAdventureLeague.Models;

public class ApplicationUser : IdentityUser
{
    public string? Fio { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Password { get; set; }
}
