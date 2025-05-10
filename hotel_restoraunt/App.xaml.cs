// hotel_restoraunt/App.xaml.cs
using System;
using System.Windows;
using hotel_restoraunt.Data;
using hotel_restoraunt.Data.Interfaces;
using hotel_restoraunt.Data.Repositories;
using hotel_restoraunt.Services;
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.Views;
using hotel_restoraunt.Views.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_restoraunt
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            
            // Налаштування обробки невідловлених виключень
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        
        private void ConfigureServices(ServiceCollection services)
        {
            
            services.AddSingleton<ILoggerService, LoggerService>();

            // Реєстрація контексту бази даних і unit of work
            services.AddSingleton<DatabaseContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Реєстрація репозиторіїв
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            // services.AddScoped<IBookingRepository, BookingRepository>();
            // services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            // services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            // Реєстрація сервісів
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            // services.AddScoped<ITableService, TableService>();
            // services.AddScoped<IOrderService, OrderService>();
            
            // Реєстрація ViewModel
            services.AddTransient<LoginViewModel>(provider => new LoginViewModel(
                provider.GetRequiredService<IUserService>(),
                provider.GetRequiredService<DatabaseService>(),
                provider.GetRequiredService<ILoggerService>()
            ));
            services.AddTransient<AdminViewModel>();
            services.AddTransient<MainViewModel>();
            
            // Реєстрація View
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginWindow>();
            // services.AddTransient<AdminWindow>();
        }
        
        // У App.xaml.cs додаємо використання логування
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            try
            {
                // Отримуємо сервіс логування
                var logger = _serviceProvider.GetService<ILoggerService>();
                logger?.LogInfo("Запуск додатку Hotel-Restaurant");
                
                // Перевіряємо підключення до бази даних
                var dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();
                logger?.LogInfo("Тестування підключення до бази даних");
                dbContext.TestConnection();
                logger?.LogInfo("Підключення до бази даних успішне");
                
                // Після успішної перевірки підключення відкриваємо вікно входу
                var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
                logger?.LogInfo("Відкриття вікна входу");
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                var logger = _serviceProvider.GetService<ILoggerService>();
                logger?.LogError("Помилка при запуску додатку", ex);
                
                MessageBox.Show($"Помилка підключення до бази даних: {ex.Message}\n\n" +
                                "Перевірте налаштування підключення та спробуйте знову.",
                                "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                
                // Тут можна додати логіку для показу вікна налаштувань БД
                ShowDatabaseSettingsWindow();
            }
        }
        
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            MessageBox.Show($"Критична помилка: {exception?.Message}\n\n" +
                            $"Програма буде закрита.", "Критична помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            
            // Логування помилки
            LogError(exception);
            
            // Завершення програми
            Current.Shutdown();
        }
        
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Помилка: {e.Exception.Message}", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            
            // Логування помилки
            LogError(e.Exception);
            
            // Позначаємо виключення як оброблене
            e.Handled = true;
        }
        
        private void LogError(Exception ex)
        {
            if (ex == null) return;
    
            try
            {
                // Спочатку спробуємо використати ILoggerService
                var logger = _serviceProvider?.GetService(typeof(ILoggerService)) as ILoggerService;
                if (logger != null)
                {
                    logger.LogError("Невідловлена помилка в додатку", ex);
                    return;
                }
        
                // Якщо ILoggerService недоступний, використовуємо стандартне логування
                string logFolderPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "Logs");
        
                if (!System.IO.Directory.Exists(logFolderPath))
                {
                    System.IO.Directory.CreateDirectory(logFolderPath);
                }
        
                string logFilePath = System.IO.Path.Combine(
                    logFolderPath, $"error_{DateTime.Now:yyyyMMdd}.log");
        
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}\n" +
                                    $"STACK TRACE: {ex.StackTrace}\n" +
                                    $"SOURCE: {ex.Source}\n" +
                                    $"-------------------------------------\n";
        
                System.IO.File.AppendAllText(logFilePath, logMessage);
            }
            catch
            {
                // Ігноруємо помилки при логуванні
            }
        }

        
        public static T GetService<T>()
        {
            if (Current is App app)
            {
                return app._serviceProvider.GetRequiredService<T>();
            }
            
            throw new InvalidOperationException("Не вдалося отримати сервіс");
        }
        
        public static void ShowDatabaseSettingsWindow()
        {
            try
            {
                var dbSettingsWindow = new DatabaseSettingsWindow();
                bool? result = dbSettingsWindow.ShowDialog();
                
                if (result == true)
                {
                    // Спробуємо підключитися з новими налаштуваннями
                    var dbContext = new DatabaseContext();
                    dbContext.TestConnection();
                    
                    MessageBox.Show("З'єднання з базою даних успішно встановлено!", 
                                    "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Перезапуск програми для застосування нових налаштувань
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Current.Shutdown();
                }
                else
                {
                    // Користувач відмовився від налаштування БД
                    MessageBox.Show("Без підключення до бази даних програма не може продовжити роботу.", 
                                   "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при налаштуванні бази даних: {ex.Message}", 
                               "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }
    }
}