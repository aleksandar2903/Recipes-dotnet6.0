using Recipes.Domain.Entities;

namespace Recipes.Application.Persistance
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
