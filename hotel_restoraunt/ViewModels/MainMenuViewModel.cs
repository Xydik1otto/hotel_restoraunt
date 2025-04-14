using hotel_restoraunt.Commands;
using hotel_restoraunt.Views;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace hotel_restoraunt.ViewModels;

public class MainMenuViewModel
{
    public ICommand OpenReservationsViewCommand { get; }
    public ICommand OpenUsersViewCommand { get; }
    public ICommand OpenRoomsViewCommand { get; }
    public ICommand OpenGuestsViewCommand { get; }

    public MainMenuViewModel()
    {
        OpenReservationsViewCommand = new RelayCommand(OpenReservationsView);
        OpenUsersViewCommand = new RelayCommand(OpenUsersView);
        OpenRoomsViewCommand = new RelayCommand(OpenRoomsView);
        OpenGuestsViewCommand = new RelayCommand(OpenGuestsView);
    }

    private void OpenReservationsView()
    {
        var reservationsView = new ReservationView();
        reservationsView.Show();
    }

    private void OpenUsersView()
    {
        var usersView = new UserView();
        usersView.Show();
    }

    private void OpenRoomsView()
    {
        var roomsView = new RoomView();
        roomsView.Show();
    }

    private void OpenGuestsView()
    {
        var guestsView = new GuestView();
        guestsView.Show();
    }
}