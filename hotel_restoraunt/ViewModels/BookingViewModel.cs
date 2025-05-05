using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Models;
using hotel_restoraunt.Services;
using hotel_restoraunt.Services.Interfaces;

namespace hotel_restoraunt.ViewModels;

public class BookingViewModel : INotifyPropertyChanged
{
    private readonly IBookingService _bookingService;

    private ObservableCollection<Booking> _bookings;
    public ObservableCollection<Booking> Bookings
    {
        get => _bookings;
        set
        {
            _bookings = value;
            OnPropertyChanged();
        }
    }

    private Booking _selectedBooking;
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
    public ICommand DeleteBookingCommand { get; }

    public BookingViewModel(IBookingService bookingService)
    {
        _bookingService = bookingService;
            
        LoadBookingsCommand = new RelayCommand(async () => await LoadBookings());
        DeleteBookingCommand = new RelayCommand(async () => await DeleteBooking());
    }

    private async Task LoadBookings()
    {
        var bookings = await _bookingService.GetAllBookings();
        Bookings = new ObservableCollection<Booking>(bookings);
    }

    private async Task DeleteBooking()
    {
        if (SelectedBooking != null)
        {
            await _bookingService.DeleteBooking(SelectedBooking.Id);
            await LoadBookings();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
        
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}