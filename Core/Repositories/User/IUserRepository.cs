namespace Core.Repositories.User;

public interface IUserRepository
{
    public Task UpdateCityAsync(Guid userId, int cityId);
}