using Infrastructure;

namespace Core.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateCityAsync(Guid userId, string city)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == default)
            throw new UserNotFoundException("User does not exist");

        user.City = city;
        _dbContext.Users.Update(user);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new DbException("User update fell.", exception);
        }
    }
}