using System.Windows;
using hotel_restoraunt.Views.ViewModels;

namespace hotel_restoraunt.Views
{
    /// <summary>
    /// Логіка взаємодії для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel(AdminViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            // Підписка на подію виходу для закриття вікна
            viewModel.LogoutRequested += (sender, args) => Close();
        }
    }
}