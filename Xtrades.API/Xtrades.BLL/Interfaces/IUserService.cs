using Xtrades.DAL.Entities;

namespace Xtrades.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User newUser);
        Task UpdateUserAsync(int id, User updatedUser);
        Task DeleteUserAsync(int id);
    }
}
