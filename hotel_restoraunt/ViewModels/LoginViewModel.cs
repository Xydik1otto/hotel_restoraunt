using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Views;
using Microsoft.Extensions.DependencyInjection;


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
    
    public ICommand LoginCommand { get; }
    
    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(_ => Login());
    }
    

    private void Login()
    {
        var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        Application.Current.Windows[0]?.Close();
    }
}

