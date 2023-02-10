namespace Core.Services.Account;

public interface IAccountService
{
    public Task<RegisterResult> RegisterAsync(NewUser newUser);
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