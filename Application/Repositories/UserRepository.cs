using Core.Repositories;
using Core.Repositories.User;
using Infrastructure;

namespace Application.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateCityAsync(Guid userId, int cityId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == default)
            throw new UserNotFoundException("User does not exist");

        user.CityExternalId = cityId;
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