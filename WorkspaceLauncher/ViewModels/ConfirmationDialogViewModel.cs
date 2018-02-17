using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class ConfirmationDialogViewModel
	{
		private ConfirmationDialogView Dialog;
		private string _message;
		private string _title;
		private int _dialogResult;


		public ConfirmationDialogViewModel(string title, string message)
		{
			_title = title;
			_message = message;
			Dialog = new ConfirmationDialogView();
			Dialog.Topmost = Configuration.AlwaysOnTop;
			Dialog.DataContext = this;
			Dialog.ShowDialog();
		}

		public int DialogResult { get { return _dialogResult; } }

		public string Message { get { return _message; } }

		public string Title { get { return _title; } }

		public ICommand CloseCommand { get { return new Command(_close); } }
		private void _close(object parameter)
		{
			_dialogResult = Convert.ToInt32(parameter);
			Dialog.Close();
		}
	}
}
