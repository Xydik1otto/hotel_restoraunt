using System.Windows;
using hotel_restoraunt.Views.ViewModels;

namespace hotel_restoraunt.Views
{
    /// <summary>
    /// Логіка взаємодії для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Підписка на подію виходу для закриття вікна
            viewModel.LogoutRequested += (sender, args) => Close();
        }
    }
}