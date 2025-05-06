using System;
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
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private readonly IReservationService _reservationService;
        private readonly IGuestService _guestService;
        private readonly IRoomService _roomService;

        public ObservableCollection<Reservation> Reservations { get; } = new();
        public ObservableCollection<Guest> Guests { get; } = new();
        public ObservableCollection<HotelRoom> Rooms { get; } = new();

        private Reservation _selectedReservation = new();
        public Reservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadReservationsCommand { get; }
        public ICommand AddReservationCommand { get; }
        public ICommand UpdateReservationCommand { get; }
        public ICommand CancelReservationCommand { get; }

        public ReservationViewModel(
            IReservationService reservationService,
            IGuestService guestService,
            IRoomService roomService)
        {
            _reservationService = reservationService;
            _guestService = guestService;
            _roomService = roomService;

            LoadReservationsCommand = new RelayCommand(async _=> await LoadReservations());
            AddReservationCommand = new RelayCommand(async _=> await AddReservation());
            UpdateReservationCommand = new RelayCommand(async _ => await UpdateReservation());
            CancelReservationCommand = new RelayCommand(async _=> await CancelReservation());

            LoadInitialData();
        }

        private async void LoadInitialData()
        {
            await LoadReservations();
            await LoadGuests();
            await LoadRooms();
        }

        private async Task LoadReservations()
        {
            var reservations = await _reservationService.GetAllReservations();
            Reservations.Clear();
            foreach (var reservation in reservations)
            {
                Reservations.Add(reservation);
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

        private async Task AddReservation()
        {
            if (SelectedReservation?.Guest == null || SelectedReservation?.Room == null) 
                return;

            await _reservationService.CreateReservation(SelectedReservation);
            await LoadReservations();
            SelectedReservation = new Reservation();
        }

        private async Task UpdateReservation()
        {
            if (SelectedReservation?.ReservationId == 0) return;
            
            await _reservationService.UpdateReservation(SelectedReservation);
            await LoadReservations();
        }

        private async Task CancelReservation()
        {
            if (SelectedReservation?.ReservationId == 0) return;
            
            await _reservationService.CancelReservation(SelectedReservation.ReservationId);
            await LoadReservations();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}