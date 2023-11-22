using Xtrades.DAL.Entities;

namespace Xtrades.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllUsersAsync(Func<User, bool> predicate);
        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User newUser);
        Task<User> UpdateUserAsync(int id, User updatedUser);
        Task DeleteUserAsync(int id);
    }
}
