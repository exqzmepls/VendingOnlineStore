namespace Core.Services.Manage;

public interface IManageService
{
    public Task<Profile?> GetProfileOrDefaultAsync();

    public Task<UpdateResult> UpdateProfileAsync(ProfileUpdate update);
}

public record Profile(string Login, string City);

public record UpdateResult
{
    private static readonly UpdateResult Success = new() { Succeeded = true };

    private static readonly UpdateResult Failed = new() { Succeeded = false };

    public bool Succeeded { get; private init; }

    public static UpdateResult SuccessResult() => Success;

    public static UpdateResult FailedResult() => Failed;
}

public record UpdateError(string Description);

public record ProfileUpdate(string City);