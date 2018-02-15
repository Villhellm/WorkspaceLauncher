using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkspaceLauncher.ViewModels
{
	public class Command : ICommand
	{
		private Action<object> _executeMethod;
		private Func<object, bool> _canExecuteMethod;

		public Command(Action<object> _executeMethod, Func<object,bool> _canExecuteMethod)
		{
			this._canExecuteMethod = _canExecuteMethod;
			this._executeMethod = _executeMethod;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_executeMethod(parameter);
		}
	}
}
