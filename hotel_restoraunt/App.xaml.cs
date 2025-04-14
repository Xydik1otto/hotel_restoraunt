using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using hotel_restoraunt.Data;
using hotel_restoraunt.Services;
using hotel_restoraunt.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace hotel_restoraunt
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // База даних
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=hotel.db"));

            // Сервіси
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();

            // Вікна
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ініціалізація БД (якщо потрібно)
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Показ головного вікна
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}