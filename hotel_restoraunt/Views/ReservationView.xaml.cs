using System.Windows;
using hotel_restoraunt.ViewModels;
using Microsoft.Extensions.DependencyInjection; 


namespace hotel_restoraunt.Views;

public partial class ReservationView : Window
{
    public ReservationView()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<ReservationViewModel>();
    }
}
