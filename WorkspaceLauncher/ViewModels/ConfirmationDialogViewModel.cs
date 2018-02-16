using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class ConfirmationDialogViewModel
	{
		private ConfirmationDialogView Dialog;
		private string _message;
		private string _title;
		private int _dialogResult;


		public ConfirmationDialogViewModel(string Title, string Message)
		{
			_title = Title;
			_message = Message;
			Dialog = new ConfirmationDialogView();
			Dialog.Topmost = Configuration.AlwaysOnTop;
			Dialog.DataContext = this;
			Dialog.ShowDialog();
		}

		public int DialogResult { get { return _dialogResult; } }

		public string Message { get { return _message; } }

		public string Title { get { return _title; } }

		public void Close(int DialogResult)
		{
			_dialogResult = DialogResult;
			Dialog.Close();
		}
	}
}
