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
		private List<string> _profiles;
		private string _selectedProfile;
		private WindowsProgram _selectedProgram;
		private List<WindowsProgram> _programs;

		public SettingsViewModel()
		{
			Profiles = Configuration.Profiles;
			if (Profiles.Count > 0)
			{
				SelectedProfile = Profiles[0];
			}

			SettingsView SettingsDialog = new SettingsView();
			SettingsDialog.Topmost = Configuration.AlwaysOnTop;
			SettingsDialog.DataContext = this;
			SettingsDialog.ShowDialog();
		}

		public string Version { get { return Configuration.Version; } }

		public Dictionary<int, string> WindowStateList
		{
			get
			{
				Dictionary<int, string> ReturnDic = new Dictionary<int, string>();
				ReturnDic.Add(1, "Normal");
				ReturnDic.Add(2, "Minimized");
				ReturnDic.Add(3, "Maximized");
				ReturnDic.Add(0, "Hide");
				return ReturnDic;
			}
		}

		public List<string> ProfilesList
		{
			get
			{
				List<string> ReturnList = new List<string>();
				ReturnList.Add("None");
				foreach (string P in Profiles)
				{
					ReturnList.Add(P);
				}
				return ReturnList;
			}
		}

		public bool CheckForUpdates
		{
			get { return Configuration.CheckForUpdates; }
			set
			{
				Configuration.CheckForUpdates = value;
				OnPropertyChanged("CheckForUpdates");
			}
		}

		public bool AlwaysOnTop
		{
			get { return Configuration.AlwaysOnTop; }
			set
			{
				Configuration.AlwaysOnTop = value;
				OnPropertyChanged("AlwaysOnTop");
			}
		}

		public string LaunchProfile
		{
			get { return Configuration.LaunchProfile; }
			set
			{
				Configuration.LaunchProfile = value;
				OnPropertyChanged("LaunchProfile");
			}
		}

		public List<string> Profiles
		{
			get { return _profiles; }
			set
			{
				_profiles = value;
				OnPropertyChanged("Profiles");
			}
		}

		public string SelectedProfile
		{
			get { return _selectedProfile; }
			set
			{
				_selectedProfile = value;
				Programs = Configuration.Programs(SelectedProfile);
				if (Programs.Count > 0)
				{
					SelectedProgram = Programs[0];
				}
				OnPropertyChanged("SelectedProfile");
				OnPropertyChanged("Programs");
			}
		}

		public List<WindowsProgram> Programs
		{
			get { return _programs; }
			set { _programs = value; }
		}

		public WindowsProgram SelectedProgram
		{
			get{return _selectedProgram;}
			set
			{
				_selectedProgram = value;
				OnPropertyChanged("SelectedProgram");
			}
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
