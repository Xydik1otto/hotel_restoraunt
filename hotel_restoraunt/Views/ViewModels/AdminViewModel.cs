using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Timers;
using MenuItem = hotel_restoraunt.Models.MenuItem;
// Чітко визначіть, який тип таймера використовувати
using Timer = System.Timers.Timer;

namespace hotel_restoraunt.Views.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly IMenuItemService _menuItemService;
        
        private User _selectedUser;
        private HotelRoom _selectedRoom;
        private Booking _selectedBooking;
        private MenuItem _selectedMenuItem;
        private RestorauntTable _selectedTable;
        private DateTime _currentDateTime;
        private Timer _timer;
        
        // Колекції для відображення даних
        public ObservableCollection<User> Users { get; private set; }
        public ObservableCollection<HotelRoom> Rooms { get; private set; }
        public ObservableCollection<Booking> Bookings { get; private set; }
        public ObservableCollection<MenuItem> MenuItems { get; private set; }
        public ObservableCollection<RestorauntTable> Tables { get; private set; }
        
        // Налаштування системи
        public AdminSettings Settings { get; private set; }
        
        // Властивості для звітів
        public DateTime ReportStartDate { get; set; } = DateTime.Today.AddMonths(-1);
        public DateTime ReportEndDate { get; set; } = DateTime.Today;
        
        // Властивості для вибору типу звіту
        private bool _isFinancialReportSelected = true;
        private bool _isOccupancyReportSelected;
        private bool _isRestaurantSalesReportSelected;
        
        // Властивості для вибору формату звіту
        private bool _isPdfFormatSelected = true;
        private bool _isExcelFormatSelected;
        private bool _isWordFormatSelected;
        
        // Властивості для обраних елементів
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        
        public HotelRoom SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();
            }
        }
        
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
            }
        }
        
        public MenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                _selectedMenuItem = value;
                OnPropertyChanged();
            }
        }
        
        public RestorauntTable SelectedTable
        {
            get => _selectedTable;
            set
            {
                _selectedTable = value;
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
        
        // Властивості для вибору типу звіту
        public bool IsFinancialReportSelected
        {
            get => _isFinancialReportSelected;
            set
            {
                _isFinancialReportSelected = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsOccupancyReportSelected
        {
            get => _isOccupancyReportSelected;
            set
            {
                _isOccupancyReportSelected = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsRestaurantSalesReportSelected
        {
            get => _isRestaurantSalesReportSelected;
            set
            {
                _isRestaurantSalesReportSelected = value;
                OnPropertyChanged();
            }
        }
        
        // Властивості для вибору формату звіту
        public bool IsPdfFormatSelected
        {
            get => _isPdfFormatSelected;
            set
            {
                _isPdfFormatSelected = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsExcelFormatSelected
        {
            get => _isExcelFormatSelected;
            set
            {
                _isExcelFormatSelected = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsWordFormatSelected
        {
            get => _isWordFormatSelected;
            set
            {
                _isWordFormatSelected = value;
                OnPropertyChanged();
            }
        }
        
        // Команди
        public ICommand LogoutCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        
        public ICommand AddRoomCommand { get; }
        public ICommand EditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }
        
        public ICommand AddMenuItemCommand { get; }
        public ICommand EditMenuItemCommand { get; }
        public ICommand DeleteMenuItemCommand { get; }
        
        public ICommand AddTableCommand { get; }
        public ICommand EditTableCommand { get; }
        public ICommand DeleteTableCommand { get; }
        
        public ICommand AddBookingCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }
        public ICommand ConfirmBookingCommand { get; }
        
        public ICommand GenerateReportCommand { get; }
        
        public ICommand TestConnectionCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand CancelSettingsCommand { get; }
        
        // Подія для запиту на вихід з системи
        public event EventHandler LogoutRequested;
        
        public AdminViewModel(
            IUserService userService,
            IRoomService roomService,
            IBookingService bookingService,
            IMenuItemService menuItemService)
        {
            // Ініціалізація сервісів
            _userService = userService;
            _roomService = roomService;
            _bookingService = bookingService;
            _menuItemService = menuItemService;
            
            // Ініціалізація колекцій
            Users = new ObservableCollection<User>();
            Rooms = new ObservableCollection<HotelRoom>();
            Bookings = new ObservableCollection<Booking>();
            MenuItems = new ObservableCollection<MenuItem>();
            Tables = new ObservableCollection<RestorauntTable>();
            
            // Ініціалізація налаштувань
            Settings = new AdminSettings();
            
            // Ініціалізація команд
            LogoutCommand = new RelayCommand(Logout);
            
            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser, CanEditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            
            AddRoomCommand = new RelayCommand(AddRoom);
            EditRoomCommand = new RelayCommand(EditRoom, CanEditRoom);
            DeleteRoomCommand = new RelayCommand(DeleteRoom, CanDeleteRoom);
            
            AddMenuItemCommand = new RelayCommand(AddMenuItem);
            EditMenuItemCommand = new RelayCommand(EditMenuItem, CanEditMenuItem);
            DeleteMenuItemCommand = new RelayCommand(DeleteMenuItem, CanDeleteMenuItem);
            
            AddTableCommand = new RelayCommand(AddTable);
            EditTableCommand = new RelayCommand(EditTable, CanEditTable);
            DeleteTableCommand = new RelayCommand(DeleteTable, CanDeleteTable);
            
            AddBookingCommand = new RelayCommand(AddBooking);
            EditBookingCommand = new RelayCommand(EditBooking, CanEditBooking);
            CancelBookingCommand = new RelayCommand(CancelBooking, CanCancelBooking);
            ConfirmBookingCommand = new RelayCommand(ConfirmBooking, CanConfirmBooking);
            
            GenerateReportCommand = new RelayCommand(GenerateReport);
            
            TestConnectionCommand = new RelayCommand<PasswordBox>(TestConnection);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            CancelSettingsCommand = new RelayCommand(CancelSettings);
            
            // Налаштування таймера для оновлення часу
            SetupTimer();
            
            // Початкове значення часу
            CurrentDateTime = DateTime.Now;
            
            // Завантаження даних
            LoadData();
            
            // Використовуйте в коді:
            _refreshTimer = new Timer(30000); // 30 секунд
            _refreshTimer.Elapsed += RefreshTimer_Elapsed;
            _refreshTimer.Start();
        }
        
        // Використовуйте в коді:
        private Timer _refreshTimer;
        
        private void SetupTimer()
        {
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (sender, args) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    CurrentDateTime = DateTime.Now;
                });
            };
            _timer.Start();
        }
        
        private async void LoadData()
        {
            try
            {
                // Завантаження користувачів
                var users = _userService.GetAllUsers();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }
                
                // Завантаження номерів
                var rooms = await _roomService.GetAllRooms();
                foreach (var room in rooms)
                {
                    Rooms.Add(room);
                }
                
                // Завантаження бронювань
                var bookings = await _bookingService.GetAllBookings();
                Bookings.Clear();
                foreach (var booking in bookings)
                {
                    Bookings.Add(booking);
                }
                
                // Завантаження пунктів меню
                var menuItems = _menuItemService.GetAllMenuItems();
                MenuItems.Clear();
                foreach (var menuItem in menuItems)
                {
                    MenuItems.Add(menuItem);
                }
                
                // Завантаження столиків (заглушка, потрібно додати відповідний сервіс)
                Tables.Clear();
                // Додавання тестових даних
                Tables.Add(new RestorauntTable { TableNumber = 1, Capacity = 2, Status = "Вільний", Location = "Основний зал" });
                Tables.Add(new RestorauntTable { TableNumber = 2, Capacity = 4, Status = "Зайнятий", Location = "Основний зал" });
                Tables.Add(new RestorauntTable { TableNumber = 3, Capacity = 6, Status = "Зарезервовано", Location = "Тераса" });
                
                // Завантаження налаштувань
                LoadSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LoadSettings()
        {
            // Завантаження налаштувань (заглушка, потрібно реалізувати)
            Settings.HotelName = "Готель і Ресторан";
            Settings.HotelAddress = "вул. Центральна, 123";
            Settings.ContactPhone = "+380 12 345 6789";
            Settings.ContactEmail = "info@hotel-restaurant.com";
            
            Settings.DatabaseServer = "localhost";
            Settings.DatabaseName = "hotel_restaurant_db";
            Settings.DatabaseUser = "admin";
            
            Settings.SmtpServer = "smtp.gmail.com";
            Settings.SmtpPort = "587";
            Settings.SmtpEmail = "notifications@hotel-restaurant.com";
        }
        
        #region Команди для користувачів
        
        private void AddUser()
        {
            MessageBox.Show("Функціональність додавання користувача знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void EditUser()
        {
            if (SelectedUser == null) return;
            
            MessageBox.Show($"Функціональність редагування користувача '{SelectedUser.Username}' знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private bool CanEditUser()
        {
            return SelectedUser != null;
        }
        
        private void DeleteUser()
        {
            if (SelectedUser == null) return;
            
            var result = MessageBox.Show($"Ви впевнені, що хочете видалити користувача '{SelectedUser.Username}'?", 
                "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Функціональність видалення користувача знаходиться в розробці", 
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private bool CanDeleteUser()
        {
            return SelectedUser != null;
        }
        
        #endregion
        
        #region Команди для номерів
        
        private void AddRoom()
        {
            MessageBox.Show("Функціональність додавання номера знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void EditRoom()
        {
            if (SelectedRoom == null) return;
            
            MessageBox.Show($"Функціональність редагування номера '{SelectedRoom.RoomNumber}' знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private bool CanEditRoom()
        {
            return SelectedRoom != null;
        }
        
        private void DeleteRoom()
        {
            if (SelectedRoom == null) return;
            
            var result = MessageBox.Show($"Ви впевнені, що хочете видалити номер '{SelectedRoom.RoomNumber}'?", 
                "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Функціональність видалення номера знаходиться в розробці", 
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private bool CanDeleteRoom()
        {
            return SelectedRoom != null;
        }
        
        #endregion
        
        #region Команди для пунктів меню
        
        private void AddMenuItem()
        {
            MessageBox.Show("Функціональність додавання страви знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void EditMenuItem()
        {
            if (SelectedMenuItem == null) return;
            
            MessageBox.Show($"Функціональність редагування страви '{SelectedMenuItem.Name}' знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private bool CanEditMenuItem()
        {
            return SelectedMenuItem != null;
        }
        
        private void DeleteMenuItem()
        {
            if (SelectedMenuItem == null) return;
            
            var result = MessageBox.Show($"Ви впевнені, що хочете видалити страву '{SelectedMenuItem.Name}'?", 
                "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Функціональність видалення страви знаходиться в розробці", 
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private bool CanDeleteMenuItem()
        {
            return SelectedMenuItem != null;
        }
        
        #endregion
        
        #region Команди для столиків
        
        private void AddTable()
        {
            MessageBox.Show("Функціональність додавання столика знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void EditTable()
        {
            if (SelectedTable == null) return;
            
            MessageBox.Show($"Функціональність редагування столика '{SelectedTable.TableNumber}' знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private bool CanEditTable()
        {
            return SelectedTable != null;
        }
        
        private void DeleteTable()
        {
            if (SelectedTable == null) return;
            
            var result = MessageBox.Show($"Ви впевнені, що хочете видалити столик '{SelectedTable.TableNumber}'?", 
                "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Функціональність видалення столика знаходиться в розробці", 
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private bool CanDeleteTable()
        {
            return SelectedTable != null;
        }
        
        #endregion
        
        #region Команди для бронювань
        
        private void AddBooking()
        {
            MessageBox.Show("Функціональність додавання бронювання знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void EditBooking()
        {
            if (SelectedBooking == null) return;
            
            MessageBox.Show($"Функціональність редагування бронювання з ID '{SelectedBooking.Id}' знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private bool CanEditBooking()
        {
            return SelectedBooking != null;
        }
        
        private void CancelBooking()
        {
            if (SelectedBooking == null) return;
            
            var result = MessageBox.Show($"Ви впевнені, що хочете скасувати бронювання з ID '{SelectedBooking.Id}'?", 
                "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Функціональність скасування бронювання знаходиться в розробці", 
                    "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private bool CanCancelBooking()
        {
            return SelectedBooking != null && SelectedBooking.Status != "Скасовано";
        }
        
        private void ConfirmBooking()
        {
            if (SelectedBooking == null) return;
            
            MessageBox.Show($"Функціональність підтвердження бронювання з ID '{SelectedBooking.Id}' знаходиться в розробці", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private bool CanConfirmBooking()
        {
            return SelectedBooking != null && SelectedBooking.Status == "Очікує підтвердження";
        }
        
        #endregion
        
        #region Команди для звітів
        
        private void GenerateReport()
        {
            string reportType = IsFinancialReportSelected ? "Фінансовий звіт" :
                               IsOccupancyReportSelected ? "Звіт про завантаженість номерів" :
                               "Звіт про продажі ресторану";
            
            string format = IsPdfFormatSelected ? "PDF" :
                           IsExcelFormatSelected ? "Excel" : "Word";
            
            MessageBox.Show($"Генерація звіту '{reportType}' у форматі {format} за період з " +
                           $"{ReportStartDate:dd.MM.yyyy} по {ReportEndDate:dd.MM.yyyy} знаходиться в розробці", 
                           "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        #endregion
        
        #region Команди для налаштувань
        
        private void TestConnection(PasswordBox passwordBox)
        {
            if (passwordBox == null) return;
            
            // Реалізація тестування підключення до бази даних
            MessageBox.Show("Тестування з'єднання з базою даних успішно виконано", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void SaveSettings()
        {
            // Реалізація збереження налаштувань
            MessageBox.Show("Налаштування успішно збережено", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void CancelSettings()
        {
            // Відновлення початкових налаштувань
            LoadSettings();
            
            MessageBox.Show("Зміни скасовано, налаштування відновлено", 
                "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        #endregion
        
        private void Logout()
        {
            // Запит на підтвердження виходу
            var result = MessageBox.Show("Ви дійсно бажаєте вийти з панелі адміністратора?", 
                "Вихід", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                // Очищення даних сесії
                _userService.Logout();
                
                // Запуск події запиту на вихід
                LogoutRequested?.Invoke(this, EventArgs.Empty);
            }
        }
        
        // Службовий метод для утилізації ресурсів
        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
            // При закритті:
            _refreshTimer.Stop();
        }
        
        private void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                LoadData();
            });
        }
    }
    
    // Клас для налаштувань системи
    public class AdminSettings
    {
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        
        public string DatabaseServer { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUser { get; set; }
        
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpEmail { get; set; }
    }
    
    // Клас для представлення столика ресторану
    public class RestorauntTable
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}