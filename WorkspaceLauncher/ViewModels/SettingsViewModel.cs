using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Models;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class SettingsViewModel : INotifyPropertyChanged
	{
		public bool CheckForUpdates { get { return Configuration.CheckForUpdates; } set { Configuration.CheckForUpdates = value; OnPropertyChanged("CheckForUpdates"); } }
		public bool AlwaysOnTop { get { return Configuration.AlwaysOnTop; } set { Configuration.AlwaysOnTop = value; OnPropertyChanged("AlwaysOnTop"); } }
		public string Version { get { return Configuration.Version; } }
		public List<string> ProfilesList
		{
			get
			{
				List<string> ReturnList = new List<string>();
				ReturnList.Add("None");
				foreach(Profile P in Profiles)
				{
					ReturnList.Add(P.ProfileName);
				}
				return ReturnList;
			}
		}
		public string LaunchProfile { get { return Configuration.LaunchProfile; } set { Configuration.LaunchProfile = value; OnPropertyChanged("LaunchProfile"); } }
		public List<Profile> Profiles { get { return Configuration.Profiles; }  }
		private Profile _selectedProfile;

		public Profile SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; OnPropertyChanged("Programs"); SelectedProgram = Programs[0]; }
		}

		public List<WindowsProgram> Programs { get { return SelectedProfile.Programs; } }
		private WindowsProgram _selectedProgram;

		public WindowsProgram SelectedProgram
		{
			get { return _selectedProgram; }
			set { _selectedProgram = value; OnPropertyChanged("WindowState"); OnPropertyChanged("WindowHeight"); OnPropertyChanged("WindowWidth"); OnPropertyChanged("WindowX"); OnPropertyChanged("WindowY"); OnPropertyChanged("LaunchPath"); OnPropertyChanged("StartArguments"); }
		}

		public Dictionary<int,string> WindowStateList
		{
			get
			{
				Dictionary<int, string> ReturnDic = new Dictionary<int, string>();
				ReturnDic.Add(1, "Normal");
				ReturnDic.Add(2, "Maximized");
				ReturnDic.Add(3, "Minimized");
				ReturnDic.Add(0, "Hide");
				return ReturnDic;
			}
		}
		public int WindowState { get { if (SelectedProgram != null) { return SelectedProgram.Status; } return 0; } set { } }
		public int WindowHeight { get { return SelectedProgram.WindowHeight; } set { } }
		public int WindowWidth { get { return SelectedProgram.WindowWidth; } set { } }
		public int WindowX { get { return SelectedProgram.XPos; } set { } }
		public int WindowY { get { return SelectedProgram.YPos; } set { } }
		public string LaunchPath { get { return SelectedProgram.StartPath; } set { } }
		public string StartArguments { get { return SelectedProgram.Argument; } set { } }

		public SettingsViewModel()
		{
			SelectedProfile = Profiles[0];
			SelectedProgram = Programs[0];
			SettingsView SettingsDialog = new SettingsView();
			SettingsDialog.DataContext = this;
			SettingsDialog.ShowDialog();
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
