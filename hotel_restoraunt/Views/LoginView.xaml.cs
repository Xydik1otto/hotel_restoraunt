using System.Windows;
using hotel_restoraunt.ViewModels;
using Microsoft.Extensions.DependencyInjection; 

namespace hotel_restoraunt.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<LoginViewModel>();
    }
}
