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
    public class GuestViewModel : INotifyPropertyChanged
    {
        private readonly IGuestService _guestService;

        public ObservableCollection<Guest> Guests { get; } = new();

        private Guest _selectedGuest = new();
        public Guest SelectedGuest
        {
            get => _selectedGuest;
            set
            {
                _selectedGuest = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadGuestsCommand { get; }
        public ICommand AddGuestCommand { get; }
        public ICommand UpdateGuestCommand { get; }
        public ICommand DeleteGuestCommand { get; }

        public GuestViewModel(IGuestService guestService)
        {
            _guestService = guestService;

            LoadGuestsCommand = new RelayCommand(async _=> await LoadGuests());
            AddGuestCommand = new RelayCommand(async _=> await AddGuest());
            UpdateGuestCommand = new RelayCommand(async _=> await UpdateGuest());
            DeleteGuestCommand = new RelayCommand(async _ => await DeleteGuest());

            LoadGuests();
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

        private async Task AddGuest()
        {
            if (string.IsNullOrEmpty(SelectedGuest.FirstName) || 
                string.IsNullOrEmpty(SelectedGuest.LastName))
                return;

            await _guestService.AddGuest(SelectedGuest);
            await LoadGuests();
            SelectedGuest = new Guest();
        }

        private async Task UpdateGuest()
        {
            if (SelectedGuest.GuestId == 0) return;
            
            await _guestService.UpdateGuest(SelectedGuest);
            await LoadGuests();
        }

        private async Task DeleteGuest()
        {
            if (SelectedGuest.GuestId == 0) return;
            
            await _guestService.DeleteGuest(SelectedGuest.GuestId);
            await LoadGuests();
            SelectedGuest = new Guest();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}