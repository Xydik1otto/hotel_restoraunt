using System.Collections.ObjectModel;
using System.ComponentModel;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services.Interfaces;
using System.Windows.Input;
using System.Runtime.CompilerServices;


namespace hotel_restoraunt.ViewModels;

public class GuestViewModel : INotifyPropertyChanged
{
    private readonly IGuestService _guestService;

    public ObservableCollection<Guest> Guests { get; set; } = new ObservableCollection<Guest>();

    private Guest _newGuest = new Guest();
    public Guest NewGuest
    {
        get => _newGuest;
        set
        {
            _newGuest = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddGuestCommand { get; }

    public GuestViewModel(IGuestService guestService)
    {
        _guestService = guestService;

        AddGuestCommand = new RelayCommand(AddGuest);
        LoadGuests();
    }

    private void AddGuest()
    {
        _guestService.AddGuest(NewGuest);
        Guests.Add(new Guest
        {
            Name = NewGuest.Name,
            Email = NewGuest.Email
        });
        NewGuest = new Guest(); // очистка форми
    }

    private void LoadGuests()
    {
        Guests.Clear();
        foreach (var guest in _guestService.GetAllGuests())
        {
            Guests.Add(guest);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}