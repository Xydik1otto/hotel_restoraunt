using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.ViewModels;

namespace hotel_restoraunt.Views;

public partial class UserView : Window
{
    public UserView()
    {
        InitializeComponent();
        DataContext = new UserViewModel(new UserService());
    }
}