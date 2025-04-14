// ViewModels/MainViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services;

namespace hotel_restoraunt.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HotelService _hotelService;
        private ObservableCollection<Room> _rooms;

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set { _rooms = value; OnPropertyChanged(nameof(Rooms)); }
        }

        public RelayCommand AddRoomCommand { get; }

        public MainViewModel(HotelService hotelService)
        {
            _hotelService = hotelService;
            Rooms = new ObservableCollection<Room>(_hotelService.GetAllRooms());
            AddRoomCommand = new RelayCommand(AddRoom);
        }

        private void AddRoom(object parameter)
        {
            var newRoom = new Room(101, "Standard", 100.0m); // Приклад
            _hotelService.AddRoom(newRoom);
            Rooms.Add(newRoom); // Оновлюємо UI через ObservableCollection
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}