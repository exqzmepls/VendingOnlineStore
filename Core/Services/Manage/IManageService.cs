namespace Core.Services.Manage;

public interface IManageService
{
    Task<IEnumerable<City>> GetCitiesAsync();
    
    public Task<Profile?> GetProfileOrDefaultAsync();

    public Task<UpdateResult> UpdateProfileAsync(ProfileUpdate update);
}

public record City(int Id, string Name);

public record Profile(string Login, int CityId);

public record UpdateResult
{
    private static readonly UpdateResult Success = new() { Succeeded = true };

    private static readonly UpdateResult Failed = new() { Succeeded = false };

    public bool Succeeded { get; private init; }

    public static UpdateResult SuccessResult() => Success;

    public static UpdateResult FailedResult() => Failed;
}

public record UpdateError(string Description);

public record ProfileUpdate(int CityId);