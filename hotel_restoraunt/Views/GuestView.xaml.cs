using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.ViewModels;

namespace hotel_restoraunt.Views;

public partial class GuestView : Window
{
    public GuestView()
    {
        InitializeComponent();
        DataContext = new GuestViewModel(new GuestService());
    }
}