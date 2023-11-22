using Moq;
using Xtrades.BLL.Services;
using Xtrades.DAL.Entities;

namespace Xtrades.Tests.ServicesTests
{
    public class UserServiceTests : BaseServiceTests
    {
        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Name = "User1" },
                new User { Id = 2, Name = "User2" },
                // Add more users as needed
            };

            UserRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedUsers);

            var userService = new UserService(UserRepositoryMock.Object);

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsers, result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            UserRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());

            var userService = new UserService(UserRepositoryMock.Object);

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenPhoneAndEmailAreUnique()
        {
            // Arrange
            var newUser = new User { Id = 1, Name = "NewUser", Phone = "380987654321", Email = "newuser@example.com" };

            UserRepositoryMock.Setup(repo => repo.CreateAsync(newUser)).ReturnsAsync(newUser);
            UserRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Func<User, bool>>())).ReturnsAsync(new User[0]);

            var userService = new UserService(UserRepositoryMock.Object);

            // Act
            var result = await userService.CreateUserAsync(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser, result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowException_WhenPhoneOrEmailAreNotUnique()
        {
            // Arrange
            var newUser = new User { Id = 1, Name = "DuplicateUser", Phone = "380987654321", Email = "duplicate@example.com" };

            UserRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Func<User, bool>>()))
                .ReturnsAsync(new[] { new User { Id = 2, Phone = "380987654321", Email = "duplicate@example.com" } });

            var userService = new UserService(UserRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => userService.CreateUserAsync(newUser));
        }
        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser_WhenPhoneAndEmailAreUnique()
        {
            // Arrange
            var userId = 1;
            var updatedUser = new User { Id = userId, Name = "UpdatedUser", Phone = "380987654321", Email = "updated@example.com" };

            UserRepositoryMock.Setup(repo => repo.ReadAsync(userId)).ReturnsAsync(new User { Id = userId, Phone = "380987654320", Email = "existing@example.com" });
            UserRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);
            UserRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Func<User, bool>>())).ReturnsAsync(new User[0]);

            var userService = new UserService(UserRepositoryMock.Object);

            // Act
            var result = await userService.UpdateUserAsync(userId, updatedUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedUser, result);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenPhoneOrEmailAreNotUnique()
        {
            // Arrange
            var userId = 1;
            var updatedUser = new User { Id = userId, Name = "DuplicateUser", Phone = "380987654321", Email = "duplicate@example.com" };

            UserRepositoryMock.Setup(repo => repo.ReadAsync(userId)).ReturnsAsync(new User { Id = userId, Phone = "380987654320", Email = "existing@example.com" });
            UserRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Func<User, bool>>()))
                .ReturnsAsync(new[] { new User { Id = 2, Phone = "380987654321", Email = "duplicate@example.com" } });

            var userService = new UserService(UserRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserAsync(userId, updatedUser));
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            var updatedUser = new User { Id = userId, Name = "UpdatedUser", Phone = "380987654321", Email = "updated@example.com" };

            UserRepositoryMock.Setup(repo => repo.ReadAsync(userId)).ReturnsAsync((User)null);

            var userService = new UserService(UserRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => userService.UpdateUserAsync(userId, updatedUser));
        }
        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;

            UserRepositoryMock.Setup(repo => repo.ReadAsync(userId)).ReturnsAsync(new User { Id = userId });
            UserRepositoryMock.Setup(repo => repo.DeleteAsync(userId)).Returns(Task.CompletedTask);

            var userService = new UserService(UserRepositoryMock.Object);

            // Act
            await userService.DeleteUserAsync(userId);

            // Assert
            UserRepositoryMock.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;

            UserRepositoryMock.Setup(repo => repo.ReadAsync(userId)).ReturnsAsync((User)null);

            var userService = new UserService(UserRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => userService.DeleteUserAsync(userId));

            // Assert
            UserRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
