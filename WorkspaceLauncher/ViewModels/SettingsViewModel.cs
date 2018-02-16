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

		public bool CheckForUpdates
		{
			get
			{
				return Configuration.CheckForUpdates;
			}
			set
			{
				Configuration.CheckForUpdates = value;
				OnPropertyChanged("CheckForUpdates");
			}
		}

		public bool AlwaysOnTop
		{
			get
			{
				return Configuration.AlwaysOnTop;
			}
			set
			{
				Configuration.AlwaysOnTop = value;
				OnPropertyChanged("AlwaysOnTop");
			}
		}

		public string Version
		{
			get
			{
				return Configuration.Version;
			}
		}

		public List<string> ProfilesList
		{
			get
			{
				List<string> ReturnList = new List<string>();
				ReturnList.Add("None");
				foreach(string P in Profiles)
				{
					ReturnList.Add(P);
				}
				return ReturnList;
			}
		}

		public string LaunchProfile
		{
			get
			{
				return Configuration.LaunchProfile;
			}
			set
			{
				Configuration.LaunchProfile = value;
				OnPropertyChanged("LaunchProfile");
			}
		}

		public List<string> Profiles
		{
			get
			{
				return _profiles;
			}

			set
			{
				_profiles = value;
				OnPropertyChanged("Profiles");
			}
		}

		public string SelectedProfile
		{
			get
			{
				return _selectedProfile;
			}

			set
			{
				_selectedProfile = value;
				OnPropertyChanged("Programs");
				OnPropertyChanged("SelectedProfile");
				SelectedProgram = Programs[0];
			}
		}

		public List<WindowsProgram> Programs
		{
			get
			{
				return Configuration.Programs(SelectedProfile);
			}
		}

		public WindowsProgram SelectedProgram
		{
			get
			{
				return _selectedProgram;
			}
			set
			{
				_selectedProgram = value;
				OnPropertyChanged("SelectedProgram");
			}
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

		public SettingsViewModel()
		{
			Profiles = Configuration.Profiles;
			SelectedProfile = Profiles[0];
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
