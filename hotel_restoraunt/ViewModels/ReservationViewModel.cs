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
    private readonly IReservationService _reservationService;

    public ObservableCollection<Reservation> Reservations { get; set; }
    public Reservation SelectedReservation { get; set; }

    public ICommand CreateCommand { get; }
    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }

    public ReservationViewModel(IReservationService reservationService)
    {
        _reservationService = reservationService;
        Reservations = new ObservableCollection<Reservation>(_reservationService.GetAllReservations());

        CreateCommand = new RelayCommand(CreateReservation);
        UpdateCommand = new RelayCommand(UpdateReservation);
        DeleteCommand = new RelayCommand(DeleteReservation);
    }

    private void CreateReservation() { /* Логіка для створення бронювання */ }
    private void UpdateReservation() { /* Логіка для редагування бронювання */ }
    private void DeleteReservation() { /* Логіка для видалення бронювання */ }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


