using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtrades.BLL.Exceptions
{
    public class GroupNotFoundException : Exception
    {
        public GroupNotFoundException(int groupId)
            : base($"Group with ID {groupId} not found")
        {
        }
    }

    public class DuplicateGroupNameException : Exception
    {
        public DuplicateGroupNameException(string groupName)
            : base($"Group with the name '{groupName}' already exists")
        {
        }
    }

    public class UserAlreadyInGroupException : Exception
    {
        public UserAlreadyInGroupException()
            : base("User is already a member of the group")
        {
        }
    }
}
