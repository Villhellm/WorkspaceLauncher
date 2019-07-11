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
		private int id;

		public int Id
		{
			get { return id; }
			set
			{
				id = value;
				OnPropertyChanged("Id");
			}
		}

		private string processName;

		public string ProcessName
		{
			get { return processName; }
			set
			{
				processName = value;
				OnPropertyChanged("ProcessName");
			}
		}
		private string startPath;

		public string StartPath
		{
			get { return startPath; }
			set
			{
				startPath = value;
				OnPropertyChanged("StartPath");
			}
		}
		private string argument;

		public string Argument
		{
			get { return argument; }
			set
			{
				argument = value;
				OnPropertyChanged("Argument");
			}
		}
		private int windowWidth;

		public int WindowWidth
		{
			get { return windowWidth; }
			set
			{
				windowWidth = value;
				OnPropertyChanged("WindowWidth");
			}
		}
		private int windowHeight;

		public int WindowHeight
		{
			get { return windowHeight; }
			set
			{
				windowHeight = value;
				OnPropertyChanged("WindowHeight");
			}
		}
		private int xPos;

		public int XPos
		{
			get { return xPos; }
			set
			{
				xPos = value;
				OnPropertyChanged("XPos");
			}
		}

		private int yPos;

		public int YPos
		{
			get { return yPos; }
			set
			{
				yPos = value;
				OnPropertyChanged("YPos");
			}
		}

		private int windowState;

		public int WindowState
		{
			get { return windowState; }
			set
			{
				windowState = value;
				OnPropertyChanged("WindowState");
			}
		}

		public static int NextId(int profileId)
		{
			var profile = Configuration.Instance.Profiles.SingleOrDefault(x => x.Id == profileId);
			if (profile.Programs.Count > 0)
			{
				return profile.Programs.OrderByDescending(x => x.id).FirstOrDefault().id++;
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
