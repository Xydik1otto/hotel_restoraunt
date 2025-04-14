using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Services;

namespace hotel_restoraunt.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private string _username;
    private string _password;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    
    public ICommand LoginCommand => new RelayCommand(Login);

    private void Login()
    {
        if (Username == "admin" && Password == "admin")
        {
            // Перехід до головного вікна
            var mainViewModel = new MainWindowViewModel(new RoomService());
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();

            // Закриваємо поточне вікно
            Application.Current.Windows[0]?.Close();
        }
    }
}

