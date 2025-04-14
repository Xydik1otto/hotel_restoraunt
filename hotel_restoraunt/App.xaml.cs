using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.ViewModels;

namespace hotel_restoraunt
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var hotelService = new HotelService();
            var mainViewModel = new MainViewModel(hotelService);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();
        }
    }
}