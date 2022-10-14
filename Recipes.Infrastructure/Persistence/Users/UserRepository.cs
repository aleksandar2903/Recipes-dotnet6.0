using Microsoft.EntityFrameworkCore;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Persistance;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Persistance.Users;

public class UserRepository : IUserRepository
{
    private readonly IDbContext _dbContext;

    public UserRepository(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>().FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }
}
