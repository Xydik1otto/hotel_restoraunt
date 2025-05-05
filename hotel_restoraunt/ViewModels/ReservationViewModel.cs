using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Runtime.CompilerServices;



namespace hotel_restoraunt.ViewModels;

public class ReservationViewModel : INotifyPropertyChanged
    {
        private readonly IGuestService _guestService;
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;

        public ObservableCollection<Guest> Guests { get; set; } = new();
        public ObservableCollection<HotelRoom> Rooms { get; set; } = new();

        private Guest _selectedGuest;
        public Guest SelectedGuest
        {
            get => _selectedGuest;
            set { _selectedGuest = value; OnPropertyChanged(); }
        }

        private HotelRoom _selectedHotelRoom;
        public HotelRoom SelectedHotelRoom
        {
            get => _selectedHotelRoom;
            set { _selectedHotelRoom = value; OnPropertyChanged(); }
        }

        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        public ICommand AddReservationCommand { get; }

        public ReservationViewModel(IGuestService guestService, IRoomService roomService, IReservationService reservationService)
        {
            _guestService = guestService;
            _roomService = roomService;
            _reservationService = reservationService;

            AddReservationCommand = new RelayCommand(AddReservation);

            LoadData();
        }

        private void LoadData()
        {
            Guests.Clear();
            foreach (var guest in _guestService.GetAllGuests())
                Guests.Add(guest);

            Rooms.Clear();
            foreach (var room in _roomService.GetAllRooms())
                Rooms.Add(room);
        }

        private void AddReservation()
        {
            if (SelectedGuest == null || SelectedHotelRoom == null) return;

            var reservation = new Reservation
            {
                Guest = SelectedGuest,
                HotelRoom = SelectedHotelRoom,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate
            };

            _reservationService.AddReservation(reservation);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

