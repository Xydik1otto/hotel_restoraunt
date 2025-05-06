using System.Windows;
using hotel_restoraunt.Services;
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModel;
using hotel_restoraunt.ViewModels;
using Microsoft.Extensions.DependencyInjection; 


namespace hotel_restoraunt.Views;

public partial class RoomView : Window
{
    public RoomView(IRoomService roomService)
    {
        InitializeComponent();
        DataContext = new RoomViewModel(roomService);
    }
}