using System.Windows;
using Microsoft.Extensions.DependencyInjection; 


namespace hotel_restoraunt.Views;

public partial class RoomView : Window
{
    public RoomView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<RoomViewModel>();
    }
}
