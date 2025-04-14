// Models/Room.cs
using System.ComponentModel;

namespace hotel_restoraunt.Models
{
    public class Room : INotifyPropertyChanged
    {
        private int _number;
        private string _type;
        private decimal _price;
        private bool _isOccupied;

        public int Number
        {
            get => _number;
            set { _number = value; OnPropertyChanged(nameof(Number)); }
        }

        public string Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(nameof(Type)); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public bool IsOccupied
        {
            get => _isOccupied;
            set { _isOccupied = value; OnPropertyChanged(nameof(IsOccupied)); }
        }

        public Room(int number, string type, decimal price)
        {
            Number = number;
            Type = type;
            Price = price;
            IsOccupied = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}