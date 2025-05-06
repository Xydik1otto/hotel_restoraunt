using hotel_restoraunt.Commands;
using System.Windows.Input;
using hotel_restoraunt.ViewModel.Base;
using hotel_restoraunt.Views;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_restoraunt.ViewModels;

public class MainMenuViewModel : ViewModelBase
{
    public ICommand OpenReservationsViewCommand { get; }
    public ICommand OpenUsersViewCommand { get; }
    public ICommand OpenRoomsViewCommand { get; }
    public ICommand OpenGuestsViewCommand { get; }

    public MainMenuViewModel()
    {
        OpenReservationsViewCommand = new RelayCommand(_ => OpenReservationsView());
        OpenUsersViewCommand = new RelayCommand(_ => OpenUsersView());
        OpenRoomsViewCommand = new RelayCommand(_ => OpenRoomsView());
        OpenGuestsViewCommand = new RelayCommand(_ => OpenGuestsView());
    }

    private void OpenReservationsView()
    {
        var reservationsView = App.ServiceProvider.GetRequiredService<ReservationView>();
        reservationsView.Show();
    }

    private void OpenUsersView()
    {
        var usersView = App.ServiceProvider.GetRequiredService<UserView>();
        usersView.Show();
    }

    private void OpenRoomsView()
    {
        var roomsView = App.ServiceProvider.GetRequiredService<RoomView>();
        roomsView.Show();
    }

    private void OpenGuestsView()
    {
        var guestsView = App.ServiceProvider.GetRequiredService<GuestView>();
        guestsView.Show();
    }
}