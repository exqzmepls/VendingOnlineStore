namespace Core.Identity;

public interface IUserIdentityProvider
{
    public Guid GetUserIdentifier();
}