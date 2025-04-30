using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UserControlsWPF
{
    // https://sourcechord.hatenablog.com/entry/2014/01/13/200039
    public class RelayCommand(Action execute) : ICommand
    {
        private readonly Func<bool>? _canExecute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute) : this(execute)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object? parameter)
        {
            execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;
        public event EventHandler? CanExecuteChanged;
        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter)
        {
            if (parameter is not T)
            {
                throw new ArgumentException("parameter is not type T.");
            }
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            if (parameter is not T)
            {
                throw new ArgumentException("parameter is not type T.");
            }
            _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
