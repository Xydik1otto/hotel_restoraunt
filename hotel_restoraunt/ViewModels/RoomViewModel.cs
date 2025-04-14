using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices; // Додано
using hotel_restoraunt.Models;
using hotel_restoraunt.Services;
using System.ComponentModel;
using System.Windows.Input;
using hotel_restoraunt.Commands;
using hotel_restoraunt.Services.Interfaces;


namespace hotel_restoraunt.ViewModels;

public class RoomViewModel : INotifyPropertyChanged
{
    private readonly IRoomService _roomService;

    public ObservableCollection<Room> Rooms { get; set; } = new ObservableCollection<Room>();

    private Room _newRoom = new Room();
    public Room NewRoom
    {
        get => _newRoom;
        set
        {
            _newRoom = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddRoomCommand { get; }

    public RoomViewModel(IRoomService roomService)
    {
        _roomService = roomService;

        AddRoomCommand = new RelayCommand(AddRoom);
        LoadRooms();
    }

    private void AddRoom()
    {
        _roomService.AddRoom(NewRoom);
        Rooms.Add(new Room
        {
            RoomNumber = NewRoom.RoomNumber,
            IsAvailable = NewRoom.IsAvailable
        });
        NewRoom = new Room(); // Очищення форми
    }

    private void LoadRooms()
    {
        Rooms.Clear();
        foreach (var room in _roomService.GetAllRooms())
        {
            Rooms.Add(room);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}