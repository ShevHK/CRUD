using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xtrades.BLL.Interfaces;
using Xtrades.DAL.Entities;
using Xtrades.DAL.Repositories;

namespace Xtrades.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.ReadAsync(id);
        }

        public async Task CreateUserAsync(User newUser)
        {
            if (await IsPhoneOrEmailUnique(newUser.Phone, newUser.Email))
            {
                throw new InvalidOperationException("User with the same phone or email already exists");
            }
            await _userRepository.CreateAsync(newUser);
        }

        public async Task UpdateUserAsync(int id, User updatedUser)
        {
            if (await IsPhoneOrEmailUnique(updatedUser.Phone, updatedUser.Email, id))
            {
                throw new InvalidOperationException("User with the same phone or email already exists");
            }
            var existingUser = await _userRepository.ReadAsync(id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            existingUser.Name = updatedUser.Name;
            existingUser.Phone = updatedUser.Phone;
            existingUser.Email = updatedUser.Email;

            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            // Перевірка існування користувача перед видаленням
            var existingUser = await _userRepository.ReadAsync(id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            await _userRepository.DeleteAsync(id);
        }
        private async Task<bool> IsPhoneOrEmailUnique(string phone, string email, int? userId = null)
        {
            // Перевірка унікальності телефону та електронної пошти
            var usersWithSamePhoneOrEmail = await _userRepository
                .GetAllAsync(u => u.Phone == phone || u.Email == email);

            if (userId.HasValue)
            {
                usersWithSamePhoneOrEmail = usersWithSamePhoneOrEmail.Where(u => u.Id != userId.Value);
            }

            return usersWithSamePhoneOrEmail.Any();
        }
    }
}
