using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using hotel_restoraunt.Commands;
using hotel_restoraunt.ViewModel.Base;

namespace hotel_restoraunt.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private ObservableCollection<HotelRoom> _rooms;
        
        public RoomViewModel(IRoomService roomService)
        {
            _roomService = roomService;
            LoadRoomsCommand = new RelayCommand(async _ => await LoadRooms());
        }

        public ObservableCollection<HotelRoom> Rooms
        {
            get => _rooms;
            set => SetField(ref _rooms, value);
        }

        public RelayCommand LoadRoomsCommand { get; }

        private async Task LoadRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            Rooms = new ObservableCollection<HotelRoom>(rooms);
        }
    }
}