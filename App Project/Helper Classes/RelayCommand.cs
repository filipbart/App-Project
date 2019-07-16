using System;
using System.Diagnostics;
using System.Windows.Input;

namespace App_Project
{
    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        private Action addItemExecute;
        private Func<bool> canExecuteAdd;

        public RelayCommand(Action<object> execute, object canExecuteAdd)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action addItemExecute, Func<bool> canExecuteAdd)
        {
            this.addItemExecute = addItemExecute;
            this.canExecuteAdd = canExecuteAdd;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters)
        {
            _execute(parameters);
        }
    }
}
