using System.Windows;
using hotel_restoraunt.Views.ViewModels;

namespace hotel_restoraunt.Views
{
    /// <summary>
    /// Логіка взаємодії для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            // Налаштовуємо події для закриття поточного вікна при відкритті основного вікна або панелі адміністратора
            viewModel.LoginSuccessful += (sender, args) => Close();
            viewModel.GuestLoginSuccessful += (sender, args) => Close();
        }
    }
}