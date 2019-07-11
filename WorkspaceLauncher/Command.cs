using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkspaceLauncher
{
	public class Command : ICommand
	{
		private Action<object> _executeMethod;

		public Command(Action<object> executeMethod)
		{
			this._executeMethod = executeMethod;
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
