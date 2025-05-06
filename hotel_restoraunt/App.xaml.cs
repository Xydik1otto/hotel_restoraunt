using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using hotel_restoraunt.Data;
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModels;
using hotel_restoraunt.Views;
using MySql.Data.MySqlClient;
using System.Data;
using hotel_restoraunt.Data.Interfaces;


namespace hotel_restoraunt
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        public App()
        {
            // Налаштування конфігурації
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Налаштування DI контейнера
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Додаємо конфігурацію
            services.AddSingleton(Configuration);

            // Налаштування підключення до БД
            services.AddScoped<IDbConnection>(provider => 
                new MySqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<DatabaseContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Реєстрація сервісів
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMenuItemService, MenuItemService>();

            // Реєстрація ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainMenuViewModel>();
            services.AddTransient<ReservationViewModel>();
            services.AddTransient<BookingViewModel>();
            services.AddTransient<GuestViewModel>();
            services.AddTransient<UserViewModel>();

            // Реєстрація View
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainMenuView>();
            services.AddTransient<ReservationView>();
            services.AddTransient<BookingView>();
            services.AddTransient<GuestView>();
            services.AddTransient<UserView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ініціалізація бази даних
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.InitDatabase().Wait();

            // Показуємо головне вікно
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}