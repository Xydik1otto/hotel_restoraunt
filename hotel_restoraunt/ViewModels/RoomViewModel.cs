using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices; // Додано
using hotel_restoraunt.Models;
using hotel_restoraunt.Services;
using System.ComponentModel;
using System.Windows.Input;
using hotel_restoraunt.Services.Interfaces;

public class RoomViewModel : INotifyPropertyChanged
{
    private readonly IRoomService _roomService;
    private string _selectedStatus;

    public ObservableCollection<Room> Rooms { get; set; }
    public ObservableCollection<Room> FilteredRooms { get; set; }

    public string SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            OnPropertyChanged(); // Оновлення даних у View
            FilterRooms();
        }
    }

    public RoomViewModel(IRoomService roomService)
    {
        _roomService = roomService;
        Rooms = new ObservableCollection<Room>(_roomService.GetAllRooms());
        FilteredRooms = new ObservableCollection<Room>(Rooms);
    }

    private void FilterRooms()
    {
        FilteredRooms.Clear();
        foreach (var room in Rooms.Where(r => r.Status == SelectedStatus || SelectedStatus == "All"))
        {
            FilteredRooms.Add(room);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}