// hotel_restoraunt/Views/ViewModels/LoginViewModel.cs
using System;
using System.Windows;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Data;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.Views.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly DatabaseService _databaseService;
        private readonly ILoggerService _logger;
        
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;
        
        // Властивості
        public string Username 
        { 
            get => _username; 
            set 
            { 
                _username = value; 
                OnPropertyChanged(); 
                ClearErrorMessage();
            } 
        }
        
        public string Password 
        { 
            get => _password; 
            set 
            { 
                _password = value; 
                OnPropertyChanged(); 
                ClearErrorMessage();
            } 
        }
        
        public string ErrorMessage 
        { 
            get => _errorMessage; 
            set 
            { 
                _errorMessage = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(HasError));
            } 
        }
        
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        
        public bool IsLoading 
        { 
            get => _isLoading; 
            set 
            { 
                _isLoading = value; 
                OnPropertyChanged(); 
            } 
        }
        
        // Команди
        public ICommand LoginCommand { get; }
        public ICommand TestConnectionCommand { get; }
        
        // Подія для сповіщення про успішну авторизацію
        public event EventHandler<bool> LoginCompleted;
        
        public LoginViewModel(IUserService userService, DatabaseService databaseService, ILoggerService logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            LoginCommand = new RelayCommand(Login, CanLogin);
            TestConnectionCommand = new RelayCommand(TestConnection);
            
            _logger.LogInfo("LoginViewModel ініціалізовано");
        }
        
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && 
                   !string.IsNullOrWhiteSpace(Password) && 
                   !IsLoading;
        }
        
        private void TestConnection()
        {
            try
            {
                _logger.LogInfo("Початок тестування підключення до бази даних");
                IsLoading = true;
                ClearErrorMessage();
                
                _databaseService.TestConnection();
                
                _logger.LogInfo("Тестування підключення до бази даних успішне");
                MessageBox.Show("Успішне підключення до бази даних", 
                    "Тест підключення", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка підключення до бази даних", ex);
                ErrorMessage = $"Помилка підключення до бази даних: {ex.Message}";
                
                MessageBox.Show($"Не вдалося підключитися до бази даних: {ex.Message}", 
                    "Помилка підключення", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        
        private void ClearErrorMessage()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = string.Empty;
            }
        }
        
        // Додаємо події, які використовуються в LoginWindow
        public event EventHandler LoginSuccessful;
        public event EventHandler GuestLoginSuccessful;
    
        // Модифікуємо метод Login для виклику подій
        private async void Login()
        {
            try
            {
                _logger.LogInfo($"Спроба входу в систему для користувача: {Username}");
                IsLoading = true;
                ClearErrorMessage();
                
                // Спроба авторизації
                var isAuthenticated = await _userService.Login(Username, Password);
                
                if (isAuthenticated)
                {
                    var currentUser = _userService.GetCurrentUser();
                    _logger.LogInfo($"Успішний вхід в систему для користувача: {Username}, тип: {currentUser.UserType}");
                    
                    // Виклик відповідної події в залежності від типу користувача
                    if (currentUser.UserType == UserType.Guest)
                    {
                        _logger.LogInfo($"Користувач {Username} увійшов як гість");
                        GuestLoginSuccessful?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        _logger.LogInfo($"Користувач {Username} увійшов як {currentUser.UserType}");
                        LoginSuccessful?.Invoke(this, EventArgs.Empty);
                    }
                    
                    // Виклик існуючої події LoginCompleted
                    LoginCompleted?.Invoke(this, true);
                }
                else
                {
                    _logger.LogWarning($"Невдала спроба входу для користувача: {Username}");
                    ErrorMessage = "Невірне ім'я користувача або пароль";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при спробі входу для користувача: {Username}", ex);
                ErrorMessage = $"Помилка авторизації: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}