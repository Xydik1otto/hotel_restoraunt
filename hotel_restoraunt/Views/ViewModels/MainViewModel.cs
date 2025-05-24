// hotel_restoraunt/Views/ViewModels/MainViewModel.cs
using System;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Services.Interfaces;
using System.Timers;
using Timer = System.Timers.Timer;

namespace hotel_restoraunt.Views.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly IUserService _userService;
        private readonly ILoggerService _logger;
        private string _userStatusText;
        private object _currentViewModel;
        private DateTime _currentDateTime;
        private System.Timers.Timer _timer;
        private bool _disposed = false;

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

        public MainViewModel(IUserService userService, ILoggerService logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Ініціалізація команд
            LogoutCommand = new RelayCommand(Logout);
            NavigateCommand = new RelayCommand<string>(Navigate);
            
            try
            {
                // Перевірка статусу користувача
                var currentUser = _userService.GetCurrentUser();
                UserStatusText = currentUser != null ? $"Авторизовано як: {currentUser.Username}" : "Гостьовий режим";
                _logger.LogInfo($"Статус користувача встановлено: {UserStatusText}");
                
                // Налаштування таймера для оновлення часу
                SetupTimer();
                
                // Початкове значення часу
                CurrentDateTime = DateTime.Now;
                
                // Початкове представлення
                CurrentViewModel = new HomeViewModel();
                _logger.LogInfo("Встановлено початкове представлення: HomeViewModel");
                
                _logger.LogInfo("MainViewModel успішно ініціалізовано");
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при ініціалізації MainViewModel", ex);
                throw;
            }
        }

        private void SetupTimer()
        {
            try
            {
                _timer = new Timer(1000);
                _timer.Elapsed += OnTimerElapsed;
                _timer.Start();
                _logger.LogDebug("Таймер оновлення часу запущено");
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при налаштуванні таймера", ex);
                throw;
            }
        }
        
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                CurrentDateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при оновленні часу", ex);
            }
        }

        private void Navigate(string destination)
        {
            try
            {
                _logger.LogInfo($"Навігація до розділу: {destination}");
                
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
                        _logger.LogWarning($"Невідомий розділ для навігації: {destination}");
                        CurrentViewModel = new HomeViewModel();
                        break;
                }
                
                _logger.LogInfo($"Навігація виконана успішно до розділу: {destination}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Помилка при навігації до розділу: {destination}", ex);
            }
        }

        private void Logout()
        {
            try
            {
                _logger.LogInfo("Запит на вихід з системи");
                
                // Вихід з системи
                _userService.Logout();
                _logger.LogInfo("Користувач вийшов із системи");
                
                // Запуск події запиту на вихід
                LogoutRequested?.Invoke(this, EventArgs.Empty);
                _logger.LogInfo("Подія виходу з системи викликана");
            }
            catch (Exception ex)
            {
                _logger.LogError("Помилка при виході з системи", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_timer != null)
                    {
                        _timer.Elapsed -= OnTimerElapsed;
                        _timer.Stop();
                        _timer.Dispose();
                        _timer = null;
                        _logger.LogInfo("Таймер зупинено та видалено");
                    }
                    
                    _logger.LogInfo("MainViewModel ресурси вивільнено");
                }
                
                _disposed = true;
            }
        }
        
        ~MainViewModel()
        {
            Dispose(false);
        }
    }

    // Заглушки для представлення різних розділів
    public class HomeViewModel : ViewModelBase { }
    public class RoomsViewModel : ViewModelBase { }
    public class RestaurantViewModel : ViewModelBase { }
    public class BookingViewModel : ViewModelBase { }
    public class HelpViewModel : ViewModelBase { }
}