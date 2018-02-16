﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
    public class ReturnStringDialogViewModel
    {
		public string Value { get; set; }

		private int _dialogResult;
		public int DialogResult { get { return _dialogResult; } }

		private string _message;
		public string Message { get { return _message; } }

		private string _title;
		public string Title { get { return _title; } }

		private ReturnStringView Dialog;

		public ReturnStringDialogViewModel(string Title, string Message)
		{
			_title = Title;
			_message = Message;
			Dialog = new ReturnStringView();
			Dialog.DataContext = this;
			Dialog.ShowDialog();
		}

		public void Close(int DialogResult)
		{
			_dialogResult = DialogResult;
			Dialog.Close();
		}
    }
}