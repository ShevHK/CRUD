using Xtrades.BLL.Exceptions;
using Xtrades.BLL.Interfaces;
using Xtrades.DAL.Entities;
using Xtrades.DAL.Repositories;

namespace Xtrades.BLL.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<UserGroup> _userGroupRepository;

        public GroupService(IRepository<Group> groupRepository, IRepository<UserGroup> userGroupRepository, IRepository<User> userRepository)
        {
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _groupRepository.GetAllAsync();
        }
        public async Task<IEnumerable<Group>> GetAllGroupsAsync(Func<Group, bool> predicate)
        {
            return await _groupRepository.GetAllAsync(predicate);
        }

        public async Task<IEnumerable<Group>> GetAllGroupsForUserAsync(int userId)
        {
            return await _groupRepository.GetAllAsync(g => g.UserGroups.Any(ug => ug.UserId == userId));
        }

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            var group = await _groupRepository.ReadAsync(id);
            if (group == null)
            {
                throw new GroupNotFoundException(id);
            }
            return group;
        }
        public async Task<Group> CreateGroupAsync(int UserId, Group newGroup)
        {
            if (newGroup != null)
            {
                if (await IsGroupNameUnique(newGroup.Name))
                {
                    var group = await _groupRepository.CreateAsync(newGroup);

                    await _userGroupRepository.CreateAsync(new UserGroup { UserId = UserId, GroupId = group.Id, IsCreator = true });
                    return group;
                }
                else
                {
                    throw new DuplicateGroupNameException(newGroup.Name);
                }
            }
            else
            {
                throw new InvalidOperationException($"New group object is null here");
            }
        }
        public async Task<Group> UpdateGroupAsync(int id, Group updatedGroup)
        {
            var existingGroup = await _groupRepository.ReadAsync(id);
            if (existingGroup == null)
            {
                throw new InvalidOperationException($"Group with ID {id} not found");
            }

            if (!(existingGroup.Name != updatedGroup.Name && await IsGroupNameUnique(updatedGroup.Name)))
            {
                throw new InvalidOperationException("Group with the same name already exists");
            }

            existingGroup.Name = updatedGroup.Name;
            existingGroup.Description = updatedGroup.Description;

            return await _groupRepository.UpdateAsync(existingGroup);
        }

        public async Task DeleteGroupAsync(int id)
        {
            var existingGroup = await _groupRepository.ReadAsync(id);
            if (existingGroup == null)
            {
                throw new InvalidOperationException($"Group with ID {id} not found");
            }

            await _userGroupRepository.DeleteAsync(ug => ug.GroupId == id);
            await _groupRepository.DeleteAsync(id);
        }


        public async Task AddUserToGroup(int userId, int groupId)
        {
            var user = await _userRepository.ReadAsync(userId);
            var group = await _groupRepository.ReadAsync(groupId);

            if (user == null || group == null)
            {
                throw new InvalidOperationException("User or group not found");
            }

            var existingUserGroup = await _userGroupRepository.GetAllAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
            if (existingUserGroup != null && existingUserGroup.Any())
            {
                throw new UserAlreadyInGroupException();
            }

            var newUserGroup = new UserGroup { UserId = userId, GroupId = groupId };
            await _userGroupRepository.CreateAsync(newUserGroup);
        }
        public async Task DeleteUserFromGroup(int userId, int groupId)
        {
            var user = await _userRepository.ReadAsync(userId);
            var group = await _groupRepository.ReadAsync(groupId);

            if (user == null || group == null)
            {
                throw new InvalidOperationException("User or group not found");
            }
            var item = (await _userGroupRepository.GetAllAsync(ug => ug.UserId == userId && ug.GroupId == groupId)).FirstOrDefault();
            if (item == null)
            {
                throw new InvalidOperationException("User is not in this group");
            }
            if (item.IsCreator)
            {
                throw new InvalidOperationException("User is Creator! You can`t remove creator from Group");
            }
            try
            {
                await _userGroupRepository.DeleteAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("User is already in this group");
            }
        }

        private async Task<bool> IsGroupNameUnique(string groupName)
        {
            return !(await _groupRepository.GetAllAsync(g => g.Name == groupName)).Any();
        }

    }
}
