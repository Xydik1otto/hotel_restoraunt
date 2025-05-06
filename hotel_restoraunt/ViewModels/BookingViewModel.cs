using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace hotel_restoraunt.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        private readonly IBookingService _bookingService;
        private readonly IGuestService _guestService;
        private readonly IRoomService _roomService;

        public ObservableCollection<Booking> Bookings { get; } = new();
        public ObservableCollection<Guest> Guests { get; } = new();
        public ObservableCollection<HotelRoom> Rooms { get; } = new();

        private Booking _selectedBooking = new();
        public Booking SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadBookingsCommand { get; }
        public ICommand AddBookingCommand { get; }
        public ICommand UpdateBookingCommand { get; }
        public ICommand DeleteBookingCommand { get; }

        public BookingViewModel(
            IBookingService bookingService,
            IGuestService guestService,
            IRoomService roomService)
        {
            _bookingService = bookingService;
            _guestService = guestService;
            _roomService = roomService;

            LoadBookingsCommand = new RelayCommand(async _ => await LoadBookings());
            AddBookingCommand = new RelayCommand(async _ => await AddBooking());
            UpdateBookingCommand = new RelayCommand(async _ => await UpdateBooking());
            DeleteBookingCommand = new RelayCommand(async _ => await DeleteBooking());

            LoadInitialData();
        }

        private async void LoadInitialData()
        {
            await LoadBookings();
            await LoadGuests();
            await LoadRooms();
        }

        private async Task LoadBookings()
        {
            var bookings = await _bookingService.GetAllBookings();
            Bookings.Clear();
            foreach (var booking in bookings)
            {
                Bookings.Add(booking);
            }
        }

        private async Task LoadGuests()
        {
            var guests = await _guestService.GetAllGuests();
            Guests.Clear();
            foreach (var guest in guests)
            {
                Guests.Add(guest);
            }
        }

        private async Task LoadRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }

        private async Task AddBooking()
        {
            if (SelectedBooking?.Guest == null || SelectedBooking?.Room == null)
                return;

            await _bookingService.CreateBooking(SelectedBooking);
            await LoadBookings();
            SelectedBooking = new Booking();
        }

        private async Task UpdateBooking()
        {
            if (SelectedBooking?.BookingId == 0) return;
            
            await _bookingService.UpdateBooking(SelectedBooking);
            await LoadBookings();
        }

        private async Task DeleteBooking()
        {
            if (SelectedBooking?.BookingId == 0) return;
            
            await _bookingService.DeleteBooking(SelectedBooking.BookingId);
            await LoadBookings();
            SelectedBooking = new Booking();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}