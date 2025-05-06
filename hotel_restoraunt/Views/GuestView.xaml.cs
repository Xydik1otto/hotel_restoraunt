using System.Windows;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_restoraunt.Views;

public partial class GuestView : Window
{
    public GuestView(IGuestService guestService)
    {
        InitializeComponent();
        DataContext = new GuestViewModel(guestService);
    }
}