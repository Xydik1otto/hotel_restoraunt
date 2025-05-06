using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace hotel_restoraunt.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;

        public ObservableCollection<User> Users { get; } = new();

        private User _selectedUser = new();
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadUsersCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public UserViewModel(IUserService userService)
        {
            _userService = userService;
            
            LoadUsersCommand = new RelayCommand(async _ => await LoadUsers());
            AddUserCommand = new RelayCommand(async _ => await AddUser());
            UpdateUserCommand = new RelayCommand(async _ => await UpdateUser());
            DeleteUserCommand = new RelayCommand(async _ => await DeleteUser());
            
            LoadUsersCommand.Execute(null);
        }

        private async Task LoadUsers()
        {
            var users = await _userService.GetAllUsers();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task AddUser()
        {
            if (string.IsNullOrEmpty(SelectedUser.Username) || 
                string.IsNullOrEmpty(SelectedUser.PasswordHash))
                return;

            await _userService.AddUser(SelectedUser);
            await LoadUsers();
            SelectedUser = new User();
        }

        private async Task UpdateUser()
        {
            if (SelectedUser.UserId == 0) return;
            
            await _userService.UpdateUser(SelectedUser);
            await LoadUsers();
        }

        private async Task DeleteUser()
        {
            if (SelectedUser.UserId == 0) return;
            
            await _userService.DeleteUser(SelectedUser.UserId);
            await LoadUsers();
            SelectedUser = new User();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}