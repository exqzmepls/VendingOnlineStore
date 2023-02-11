namespace Core.Repositories.User;

public interface IUserRepository
{
    public Task UpdateCityAsync(Guid userId, string city);
}