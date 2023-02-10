namespace Core.Services.Account;

public interface IAccountService
{
    public Task<RegisterResult> RegisterAsync(NewUser newUser);

    public Task<LoginResult> LoginAsync(LoginDetails loginDetails);
}

public record RegisterResult
{
    private static readonly RegisterResult Success = new() { Succeeded = true };

    private readonly List<RegisterError> _errors = new();

    public bool Succeeded { get; private init; }

    public IEnumerable<RegisterError> Errors => _errors.AsEnumerable();

    public static RegisterResult SuccessResult() => Success;

    public static RegisterResult FailedResult(IEnumerable<RegisterError> errors)
    {
        var result = new RegisterResult
        {
            Succeeded = false
        };
        result._errors.AddRange(errors);
        return result;
    }
}

public record RegisterError(string Description);

public record NewUser(string Login, string City, string Password);

public record LoginResult
{
    private static readonly LoginResult Success = new() { Succeeded = true };
    private static readonly LoginResult Failed = new() { Succeeded = false };

    public bool Succeeded { get; protected init; }

    public static LoginResult SuccessResult() => Success;

    public static LoginResult FailedResult() => Failed;
}

public record LoginDetails(string Login, string Password);