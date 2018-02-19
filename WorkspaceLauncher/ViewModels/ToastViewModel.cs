using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class ToastViewModel
	{
		ToastView _toastWindow;
		private string _toastMessage;
		DispatcherTimer _closer;

		public ToastViewModel(string ToastMessage, int ShowTime)
		{
			_closer = new DispatcherTimer();
			_closer.Interval = new TimeSpan(0,0,0,0,ShowTime);
			_closer.Tick += _closer_Tick;
			_closer.IsEnabled = true;
			this.ToastMessage = ToastMessage;
			_toastWindow = new ToastView();
			_toastWindow.DataContext = this;
			_toastWindow.Show();
			_closer.Start();
		}

		private void _closer_Tick(object sender, EventArgs e)
		{
			_closer.Stop();
			_toastWindow.Close();
		}

		public string ToastMessage
		{
			get { return _toastMessage; }
			set { _toastMessage = value; }
		}

		public static void Show(string ToastMessage, int ShowTime = 1500)
		{
			Task.Factory.StartNew(new Action(() =>
			{
				ToastViewModel asdf = new ToastViewModel(ToastMessage, ShowTime);
			}), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
