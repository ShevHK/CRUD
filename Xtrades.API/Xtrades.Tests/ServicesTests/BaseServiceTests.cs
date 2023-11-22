using Moq;
using Xtrades.BLL.Interfaces;
using Xtrades.DAL.Entities;
using Xtrades.DAL.Repositories;

namespace Xtrades.Tests.ServicesTests
{
    public class BaseServiceTests
    {
        protected readonly Mock<IRepository<Group>> GroupRepositoryMock;
        protected readonly Mock<IRepository<UserGroup>> UserGroupRepositoryMock;
        protected readonly Mock<IRepository<User>> UserRepositoryMock;
        protected readonly Mock<IUserService> UserServiceMock;
        protected readonly Mock<IGroupService> GroupServiceMock;

        protected BaseServiceTests()
        {
            // Ініціалізуємо моки для репозиторіїв
            GroupRepositoryMock = new Mock<IRepository<Group>>();
            UserGroupRepositoryMock = new Mock<IRepository<UserGroup>>();
            UserRepositoryMock = new Mock<IRepository<User>>();
            UserServiceMock = new Mock<IUserService>();
            GroupServiceMock = new Mock<IGroupService>();
        }
    }
}
