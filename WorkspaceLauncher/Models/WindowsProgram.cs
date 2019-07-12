using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WorkspaceLauncher.Models
{
	public class WindowsProgram : INotifyPropertyChanged
	{
		private int _id;
		private string _processName;
		private string _startPath;
		private string _argument;
		private int _windowWidth;
		private int _windowHeight;
		private int _xPos;
		private int _yPos;
		private int _windowState;

		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				OnPropertyChanged("Id");
			}
		}

		public string ProcessName
		{
			get { return _processName; }
			set
			{
				_processName = value;
				OnPropertyChanged("ProcessName");
			}
		}

		public string StartPath
		{
			get { return _startPath; }
			set
			{
				_startPath = value;
				OnPropertyChanged("StartPath");
			}
		}

		public string Argument
		{
			get { return _argument; }
			set
			{
				_argument = value;
				OnPropertyChanged("Argument");
			}
		}

		public int WindowWidth
		{
			get { return _windowWidth; }
			set
			{
				_windowWidth = value;
				OnPropertyChanged("WindowWidth");
			}
		}

		public int WindowHeight
		{
			get { return _windowHeight; }
			set
			{
				_windowHeight = value;
				OnPropertyChanged("WindowHeight");
			}
		}

		public int XPos
		{
			get { return _xPos; }
			set
			{
				_xPos = value;
				OnPropertyChanged("XPos");
			}
		}

		public int YPos
		{
			get { return _yPos; }
			set
			{
				_yPos = value;
				OnPropertyChanged("YPos");
			}
		}

		public int WindowState
		{
			get { return _windowState; }
			set
			{
				_windowState = value;
				OnPropertyChanged("WindowState");
			}
		}

		public static int NextId(int profileId)
		{
			var profile = Configuration.Instance.Profiles.SingleOrDefault(x => x.Id == profileId);
			if (profile.Programs.Count > 0)
			{
				return profile.Programs.OrderByDescending(x => x._id).FirstOrDefault()._id++;
			}
			return 1;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string Property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(Property));
			}
		}
	}
}
