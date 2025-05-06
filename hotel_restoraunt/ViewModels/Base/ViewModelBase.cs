using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using hotel_restoraunt.Commands;

namespace hotel_restoraunt.ViewModel.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected ICommand CreateCommand(Action execute, Func<bool> canExecute = null)
        {
            return new RelayCommand(_ => execute(), _ => canExecute?.Invoke() ?? true);
        }

        protected ICommand CreateCommand<T>(Action<T> execute, Func<T, bool> canExecute = null)
        {
            return new RelayCommand<T>(execute, canExecute);
        }

        protected ICommand CreateAsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            return CreateCommand(async () => await execute(), canExecute);
        }

        protected ICommand CreateAsyncCommand<T>(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            return new RelayCommand<T>(async param => await execute(param), canExecute);
        }
    }
}