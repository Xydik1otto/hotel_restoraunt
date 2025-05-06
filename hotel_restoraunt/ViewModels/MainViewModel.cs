// Додайте ці using-директиви:
using hotel_restoraunt.Services.Implementations;
using hotel_restoraunt.Services.Interfaces;
using hotel_restoraunt.ViewModel.Base;

public class MainViewModel : ViewModelBase
{
    private readonly IRoomService _roomService;
    
    public MainViewModel(IRoomService roomService)
    {
        _roomService = roomService;
    }
}