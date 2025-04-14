using System.Windows;
using hotel_restoraunt.ViewModels;

namespace hotel_restoraunt.Views;

public partial class MainMenuView : Window
{
    public MainMenuView()
    {
        InitializeComponent();
        DataContext = new MainMenuViewModel();
    }
}