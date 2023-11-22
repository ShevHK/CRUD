using Xtrades.DAL.Entities;

namespace Xtrades.BLL.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroupsAsync();
        Task<IEnumerable<Group>> GetAllGroupsAsync(Func<Group, bool> predicate);

        Task<IEnumerable<Group>> GetAllGroupsForUserAsync(int id);
        Task<Group> GetGroupByIdAsync(int id);

        Task<Group> CreateGroupAsync(int UserId,Group newUser);
        Task<Group> UpdateGroupAsync(int id, Group updatedUser);
        Task DeleteGroupAsync(int id);



        Task AddUserToGroup(int userId,int groupId);
        Task DeleteUserFromGroup(int userId,int groupId);
    }
}
