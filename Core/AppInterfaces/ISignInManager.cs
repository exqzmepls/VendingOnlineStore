namespace Core.AppInterfaces;

public interface ISignInManager<in TUser> where TUser : class
{
    public Task SignInAsync(TUser user);
    public Task<SignInResult> PasswordSignInAsync(string login, string password);
}

public record SignInResult
{
    private static readonly SignInResult Success = new() { Succeeded = true };
    private static readonly SignInResult Failed = new() { Succeeded = false };

    public bool Succeeded { get; protected init; }

    public static SignInResult SuccessResult() => Success;

    public static SignInResult FailedResult() => Failed;
}