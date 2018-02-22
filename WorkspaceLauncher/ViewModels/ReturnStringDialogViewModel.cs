using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class ReturnStringDialogViewModel : INotifyPropertyChanged
	{
		private int _dialogResult;
		private string _title;
		private string _message;
		private string _value;
		private string _buttonText;
		private bool _hasButton;
		private ReturnStringView Dialog;

		public ReturnStringDialogViewModel(string Title, string Message, string Value = "", bool HasButton = false, string ButtonText = "")
		{
			_title = Title;
			_message = Message;
			_buttonText = ButtonText;
			_hasButton = HasButton;
			this.Value = Value;
			Dialog = new ReturnStringView();
			Dialog.Topmost = Configuration.AlwaysOnTop;
			Dialog.DataContext = this;
			Dialog.ShowDialog();
		}
		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public int DialogResult { get { return _dialogResult; } }

		public string Message { get { return _message; } }

		public string Title { get { return _title; } }

		public string ButtonText { get { return _buttonText; } }

		public bool HasButton { get { return _hasButton; } }

		public Command ButtonCommand { get { return new Command(_button); } }
		private void _button(object parameter)
		{
			Microsoft.Win32.OpenFileDialog fileSelector = new Microsoft.Win32.OpenFileDialog();
			if (fileSelector.ShowDialog() == true && fileSelector.CheckFileExists)
			{
				Value = fileSelector.FileName;
			}

		}

		public Command CloseCommand { get { return new Command(_close); } }
		private void _close(object parameter)
		{
			_dialogResult = Convert.ToInt32(parameter);
			Dialog.Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string PropertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
	}
}
