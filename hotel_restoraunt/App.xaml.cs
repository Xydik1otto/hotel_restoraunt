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

        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            ServiceProvider = _serviceProvider;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // База даних
            services.AddDbContext<AppDbContext>(options =>
                options.UseSql("Data Source=hotel.db"));

            // Сервіси
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IUserService, UserService>();

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainMenuViewModel>();

            // Views
            services.AddSingleton<MainWindow>();
            services.AddTransient<MainMenuView>();
            
            services.AddDbContext<AppDbContext>();
            services.AddTransient<BookingService>();
            services.AddTransient<BookingViewModel>();
            services.AddTransient<IBookingService, BookingService>();
            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var mainMenu = _serviceProvider.GetRequiredService<MainMenuView>();
            mainMenu.Show();
        }
    }
}