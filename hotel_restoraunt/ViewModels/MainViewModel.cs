// ViewModels/MainViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services;


namespace hotel_restoraunt.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<HotelRoom> Rooms { get; }
        private readonly RoomService _roomService;

        public MainWindowViewModel(RoomService roomService)
        {
            _roomService = roomService;
            Rooms = new ObservableCollection<HotelRoom>(_roomService.GetAllRooms());
        }
    }
}