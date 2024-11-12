using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContactBook.Core.Helpers
{
    internal class RelayCommandWithParameter : ICommand
    {
        private Action<object> mAction;

        public RelayCommandWithParameter(Action<object> action)
        {
            mAction = action ?? throw new ArgumentNullException(nameof(action));
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            mAction(parameter);
        }
    }
}
