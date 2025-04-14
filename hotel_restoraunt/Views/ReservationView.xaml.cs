using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.ViewModels;
using Microsoft.Extensions.DependencyInjection; 


namespace hotel_restoraunt.Views;

public partial class ReservationView : Window
{
    public ReservationView()
    {
        InitializeComponent();
        DataContext = new ReservationViewModel(
            new GuestService(),
            new RoomService(),
            new ReservationService()
        );
    }
}
