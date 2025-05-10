// hotel_restoraunt/Services/Implementations/UserService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private User _currentUser;
        
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        
        // Реалізація методів IUserService
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            bool isAuthenticated = await _userRepository.AuthenticateUser(username, password);
            if (isAuthenticated)
            {
                var user = await _userRepository.GetUserByUsername(username);
                _currentUser = user;
                return user;
            }
            return null;
        }
        
        public async Task<bool> UsernameExistsAsync(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
            return user != null;
        }
        
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }
        
        public async Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType)
        {
            var allUsers = await _userRepository.GetAllUsers();
            return allUsers.Where(u => u.UserType == userType);
        }
        
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                return false;
                
            // Перевірка поточного пароля
            bool isAuthenticated = await _userRepository.AuthenticateUser(user.Username, currentPassword);
            if (!isAuthenticated)
                return false;
                
            // Оновлення пароля
            user.Password = newPassword; // Тут має бути хешування пароля
            return await _userRepository.UpdateUser(user);
        }
        
        public async Task<bool> UpdateUserStatusAsync(int userId, UserStatus status)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
                return false;
                
            user.Status = status;
            return await _userRepository.UpdateUser(user);
        }
        
        // Методи для поточного користувача
        public async Task<bool> Login(string username, string password)
        {
            var user = await AuthenticateAsync(username, password);
            return user != null;
        }
        
        public void Logout()
        {
            _currentUser = null;
        }
        
        public User GetCurrentUser()
        {
            return _currentUser;
        }
        
        // Реалізація IService<User>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllUsers();
        }
        
        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetUserById(id);
        }
        
        // Виправлено тип повернення на Task<User>
        public async Task<User> AddAsync(User user)
        {
            int userId = await _userRepository.AddUser(user);
            if (userId > 0)
            {
                // Повертаємо доданого користувача з ID
                user.Id = userId;
                return user;
            }
            return null;
        }
        
        // Виправлено тип повернення на Task
        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateUser(user);
        }
        
        // Виправлено тип повернення на Task
        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteUser(id);
        }
        
        public async Task<bool> ExistsAsync(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return user != null;
        }
        
        // Застарілі методи, які слід помітити як [Obsolete] або видалити
        [Obsolete("Використовуйте асинхронну версію GetAllAsync() замість цього методу")]
        public IEnumerable<User> GetAllUsers()
        {
            return GetAllAsync().Result;
        }
        
        [Obsolete("Використовуйте асинхронну версію GetByIdAsync() замість цього методу")]
        public async Task<User> GetUserById(int id)
        {
            return await GetByIdAsync(id);
        }
        
        [Obsolete("Використовуйте асинхронну версію AddAsync() замість цього методу")]
        public async Task<int> AddUser(User user)
        {
            var addedUser = await AddAsync(user);
            return addedUser?.Id ?? 0;
        }
        
        [Obsolete("Використовуйте асинхронну версію UpdateAsync() замість цього методу")]
        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                await UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        [Obsolete("Використовуйте асинхронну версію DeleteAsync() замість цього методу")]
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                await DeleteAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}