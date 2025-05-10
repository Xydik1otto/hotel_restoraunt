// hotel_restoraunt/Views/DatabaseSettingsWindow.xaml.cs
using System.Windows;
using hotel_restoraunt.Data;

namespace hotel_restoraunt.Views
{
    public partial class DatabaseSettingsWindow : Window
    {
        private DatabaseConfig _config;
        
        public DatabaseSettingsWindow()
        {
            InitializeComponent();
            LoadDatabaseConfig();
        }
        
        private void LoadDatabaseConfig()
        {
            _config = DatabaseConfig.LoadConfig();
            
            txtServer.Text = _config.Server;
            txtDatabase.Text = _config.Database;
            txtPort.Text = _config.Port.ToString();
            txtUsername.Text = _config.UserId;
            txtPassword.Password = _config.Password;
        }
        
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _config.Server = txtServer.Text;
                _config.Database = txtDatabase.Text;
                _config.UserId = txtUsername.Text;
                _config.Password = txtPassword.Password;
                
                if (int.TryParse(txtPort.Text, out int port))
                {
                    _config.Port = port;
                }
                
                DatabaseConfig.SaveConfig(_config);
                
                MessageBox.Show("Налаштування збережено успішно", "Інформація", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Помилка збереження налаштувань: {ex.Message}", "Помилка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Створюємо тимчасовий об'єкт конфігурації з поточними значеннями
                var testConfig = new DatabaseConfig
                {
                    Server = txtServer.Text,
                    Database = txtDatabase.Text,
                    UserId = txtUsername.Text,
                    Password = txtPassword.Password
                };
                
                if (int.TryParse(txtPort.Text, out int port))
                {
                    testConfig.Port = port;
                }
                
                // Створюємо тимчасове з'єднання для тестування
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(testConfig.ConnectionString))
                {
                    connection.Open();
                    MessageBox.Show("З'єднання з базою даних успішно встановлено!", "Тест підключення", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка підключення до бази даних: {ex.Message}", "Помилка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}