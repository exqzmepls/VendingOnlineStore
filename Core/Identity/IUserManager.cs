namespace Core.Identity;

public interface IUserManager
{
    public Task<UserDetails?> GetUserAsync();
}

public record UserDetails(Guid id, string Login, string City);