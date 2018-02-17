using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class ReturnStringDialogViewModel
	{
		private int _dialogResult;
		private string _title;
		private string _message;
		private ReturnStringView Dialog;

		public ReturnStringDialogViewModel(string Title, string Message)
		{
			_title = Title;
			_message = Message;
			Dialog = new ReturnStringView();
			Dialog.Topmost = Configuration.AlwaysOnTop;
			Dialog.DataContext = this;
			Dialog.ShowDialog();
		}
		public string Value { get; set; }

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
