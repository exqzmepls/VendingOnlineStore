namespace Core.AppInterfaces;

public interface ISignInManager<in TUser> where TUser : class
{
    public Task SignInAsync(TUser user);
}