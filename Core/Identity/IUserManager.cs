namespace Core.Identity;

public interface IUserManager
{
    public Task<UserDetails?> GetUserOrDefaultAsync();
}

public record UserDetails(string Login, string City);