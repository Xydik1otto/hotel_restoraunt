using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModels;

namespace hotel_restoraunt.Views;

public partial class UserView : Window
{
    public UserView(IUserService userService)
    {
        InitializeComponent();
        DataContext = new UserViewModel(userService);
    }
}