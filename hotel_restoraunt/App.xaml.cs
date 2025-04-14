using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using hotel_restoraunt.Data;
using hotel_restoraunt.Services;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModels;
using hotel_restoraunt.Views;
using Microsoft.EntityFrameworkCore;
namespace hotel_restoraunt
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public static ServiceProvider ServiceProvider { get; private set; } // Додано для доступу

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            ServiceProvider = _serviceProvider; // Ініціалізація
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Налаштування ваших сервісів
            services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=hotel.db"));
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();

            // Додати ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();

            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}