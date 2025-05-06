using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModels;
using Microsoft.Extensions.DependencyInjection; 


namespace hotel_restoraunt.Views;

public partial class ReservationView : Window
{
    public ReservationView(
        IReservationService reservationService,
        IGuestService guestService,
        IRoomService roomService)
    {
        InitializeComponent();
        DataContext = new ReservationViewModel(reservationService, guestService, roomService);
    }
}
