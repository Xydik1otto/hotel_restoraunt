using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.Views;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_restoraunt.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;
        
        private string _username;
        private string _errorMessage;
        private bool _hasError;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                HasError = !string.IsNullOrEmpty(value);
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand GuestLoginCommand { get; }

        // Події, які сигналізують про успішний вхід
        public event EventHandler LoginSuccessful;
        public event EventHandler GuestLoginSuccessful;

        public LoginViewModel(IUserService userService, IServiceProvider serviceProvider)
        {
            _userService = userService;
            _serviceProvider = serviceProvider;
            
            LoginCommand = new RelayCommand<PasswordBox>(Login);
            GuestLoginCommand = new RelayCommand(LoginAsGuest);
        }

        private void Login(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Будь ласка, введіть логін";
                return;
            }

            if (passwordBox == null || string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                ErrorMessage = "Будь ласка, введіть пароль";
                return;
            }

            // Перевірка логіну та пароля (тут можна додати перевірку з бази даних)
            if (Username.ToLower() == "admin" && passwordBox.Password == "admin")
            {
                // Відкриття панелі адміністратора
                var adminPanel = _serviceProvider.GetRequiredService<AdminPanel>();
                adminPanel.Show();
                
                // Запускаємо подію успішного входу
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // Перевірка звичайного користувача через сервіс
                var isValidUser = _userService.ValidateUser(Username, passwordBox.Password);
                
                if (isValidUser)
                {
                    // Відкриття основного вікна для зареєстрованого користувача
                    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                    
                    // Запускаємо подію успішного входу
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Невірний логін або пароль";
                }
            }
        }

        private void LoginAsGuest()
        {
            // Відкриття основного вікна для гостя
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            
            // Запускаємо подію успішного входу в режимі гостя
            GuestLoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
    }
}
