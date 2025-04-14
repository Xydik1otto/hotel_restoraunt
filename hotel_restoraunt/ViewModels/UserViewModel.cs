using System.Collections.ObjectModel;
using System.ComponentModel;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace hotel_restoraunt.ViewModels;

public class UserViewModel : INotifyPropertyChanged
{
    private readonly IUserService _userService;

    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

    private User _newUser = new User();
    public User NewUser
    {
        get => _newUser;
        set
        {
            _newUser = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddUserCommand { get; }
    public ICommand EditUserCommand { get; }
    public ICommand DeleteUserCommand { get; }

    public UserViewModel(IUserService userService)
    {
        _userService = userService;

        AddUserCommand = new RelayCommand(AddUser);
        EditUserCommand = new RelayCommand(EditUser);
        DeleteUserCommand = new RelayCommand(DeleteUser);

        LoadUsers();
    }

    private void LoadUsers()
    {
        Users.Clear();
        foreach (var user in _userService.GetAllUsers())
        {
            Users.Add(user);
        }
    }

    private void AddUser()
    {
        _userService.AddUser(NewUser);
        Users.Add(NewUser);
        NewUser = new User(); // очистка форми
    }

    private void EditUser()
    {
        if (NewUser == null) return;
        _userService.UpdateUser(NewUser);
    }

    private void DeleteUser()
    {
        if (NewUser == null) return;
        _userService.DeleteUser(NewUser.Id);
        Users.Remove(NewUser);
        NewUser = new User(); // очистка форми
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}