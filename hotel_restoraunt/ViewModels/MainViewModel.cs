using System;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Services.Interfaces;
using System.Timers;

namespace hotel_restoraunt.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private string _userStatusText;
        private object _currentViewModel;
        private DateTime _currentDateTime;
        private Timer _timer;

        public string UserStatusText
        {
            get => _userStatusText;
            set
            {
                _userStatusText = value;
                OnPropertyChanged();
            }
        }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand NavigateCommand { get; }

        // Подія для запиту на вихід з системи
        public event EventHandler LogoutRequested;

        public MainViewModel(IUserService userService)
        {
            _userService = userService;
            
            // Ініціалізація команд
            LogoutCommand = new RelayCommand(Logout);
            NavigateCommand = new RelayCommand<string>(Navigate);
            
            // Перевірка статусу користувача
            var currentUser = _userService.GetCurrentUser();
            UserStatusText = currentUser != null ? $"Авторизовано як: {currentUser.Username}" : "Гостьовий режим";
            
            // Налаштування таймера для оновлення часу
            SetupTimer();
            
            // Початкове значення часу
            CurrentDateTime = DateTime.Now;
            
            // Початкове представлення
            CurrentViewModel = new HomeViewModel();
        }

        private void SetupTimer()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += (sender, args) => 
            {
                CurrentDateTime = DateTime.Now;
            };
            _timer.Start();
        }

        private void Navigate(string destination)
        {
            switch (destination)
            {
                case "Home":
                    CurrentViewModel = new HomeViewModel();
                    break;
                case "Rooms":
                    CurrentViewModel = new RoomsViewModel();
                    break;
                case "Restaurant":
                    CurrentViewModel = new RestaurantViewModel();
                    break;
                case "Booking":
                    CurrentViewModel = new BookingViewModel();
                    break;
                case "Help":
                    CurrentViewModel = new HelpViewModel();
                    break;
                default:
                    CurrentViewModel = new HomeViewModel();
                    break;
            }
        }

        private void Logout()
        {
            // Вихід з системи
            _userService.Logout();
            
            // Запуск події запиту на вихід
            LogoutRequested?.Invoke(this, EventArgs.Empty);
        }

        // Службовий метод для утилізації ресурсів
        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }

    // Заглушки для представлення різних розділів
    public class HomeViewModel : ViewModelBase { }
    public class RoomsViewModel : ViewModelBase { }
    public class RestaurantViewModel : ViewModelBase { }
    public class BookingViewModel : ViewModelBase { }
    public class HelpViewModel : ViewModelBase { }
}
