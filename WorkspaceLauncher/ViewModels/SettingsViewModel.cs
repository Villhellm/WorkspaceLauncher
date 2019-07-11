using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Models;
using WorkspaceLauncher.Views;
using System.Collections.ObjectModel;

namespace WorkspaceLauncher.ViewModels
{
	public class SettingsViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<Profile> Profiles
		{
			get
			{
				return new ObservableCollection<Profile>(Configuration.Instance.Profiles);
			}
		}


		public bool AlwaysOnTop
		{
			get { return Configuration.Instance.AlwaysOnTop; }
			set
			{
				Configuration.Instance.AlwaysOnTop = value;
			}
		}

		public bool CheckForUpdates
		{
			get { return Configuration.Instance.CheckForUpdates; }
			set
			{
				Configuration.Instance.CheckForUpdates = value;
			}
		}

		public int LaunchProfileId
		{
			get { return Configuration.Instance.LaunchProfileId; }
			set
			{
				Configuration.Instance.LaunchProfileId = value;
			}
		}



		private Profile _selectedProfile;
		private WindowsProgram _selectedProgram;
		public ObservableCollection<WindowsProgram> Programs
		{
			get
			{
				if(SelectedProfile != null)
				{
					return new ObservableCollection<WindowsProgram>(SelectedProfile.Programs);
				}
				return null;
			}
		}

		public SettingsViewModel()
		{
			SettingsView SettingsDialog = new SettingsView();
			SettingsDialog.Topmost = Configuration.Instance.AlwaysOnTop;
			SettingsDialog.DataContext = this;
			SettingsDialog.ShowDialog();
		}

		public bool ProgramSelected { get { return SelectedProgram != null; } }

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

		public Profile SelectedProfile
		{
			get { return _selectedProfile; }
			set
			{
				_selectedProfile = value;
				if (Programs.Count > 0)
				{
					SelectedProgram = Programs[0];
				}
				OnPropertyChanged("SelectedProfile");
				OnPropertyChanged("Programs");
			}
		}

		public WindowsProgram SelectedProgram
		{
			get { return _selectedProgram; }
			set
			{
				_selectedProgram = value;
				OnPropertyChanged("SelectedProgram");
				OnPropertyChanged("ProgramSelected");
			}
		}

		public Command DeleteProfileCommand { get { return new Command(_deleteProfile); } }
		private void _deleteProfile(object parameter)
		{
			if (SelectedProfile != null)
			{
				Configuration.Instance.Profiles.Remove(SelectedProfile);

				if (Profiles.Count > 0)
				{
					SelectedProfile = Profiles[0];
				}

			}
		}

		public Command RenameProfileCommand { get { return new Command(_renameProfile); } }
		private void _renameProfile(object parameter)
		{
			if (SelectedProfile != null)
			{
				ReturnStringDialogViewModel renamer = new ReturnStringDialogViewModel("Rename Profile", "Please choose a new name for profile: " + SelectedProfile.Name);
				if(renamer.DialogResult == 1)
				{
					SelectedProfile.Name = renamer.Value;
				}
			}
		}

		public Command AddProgramsCommand { get { return new Command(_addPrograms); } }
		private void _addPrograms(object parameter)
		{
			if (SelectedProfile != null)
			{
				SelectProcessesViewModel SelectNew = new SelectProcessesViewModel(SelectedProfile.Id);

				if (Programs.Count > 0)
				{
					SelectedProgram = Programs[0];
				}
			}
		}

		public Command ClearLaunchProfile { get { return new Command(_clearLaunchProfile); } }
		private void _clearLaunchProfile(object parameter)
		{
			LaunchProfileId = 0;
			OnPropertyChanged("LaunchProfileId");
		}

		public Command RemoveProgramCommand { get { return new Command(_removeProgram); } }
		private void _removeProgram(object parameter)
		{
			if (SelectedProfile != null && SelectedProgram != null)
			{
				SelectedProfile.Programs.Remove(SelectedProgram);
				if (Programs.Count > 0)
				{
					SelectedProgram = Programs[0];
				}
				OnPropertyChanged("Programs");

			}
		}

		public Command CheckForUpdatesCommand { get { return new Command(_checkForUpdates); } }
		private void _checkForUpdates(object parameter)
		{
			GithubUpdater Updater = new GithubUpdater();
			if(Updater.LaunchUpdater() == 0)
			{
				ToastViewModel.Show("No update available");
			}
		}

		public Command FindFileCommand { get { return new Command(_findFile); } }
		private void _findFile(object parameter)
		{
			if(SelectedProgram != null)
			{
				Microsoft.Win32.OpenFileDialog fileSelector = new Microsoft.Win32.OpenFileDialog();
				if(fileSelector.ShowDialog() == true && fileSelector.CheckFileExists)
				{
					SelectedProgram.StartPath = fileSelector.FileName;
					OnPropertyChanged("SelectedProgram");
				}
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
