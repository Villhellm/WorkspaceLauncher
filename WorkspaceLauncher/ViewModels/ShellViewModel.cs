
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkspaceLauncher.Models;
using WorkspaceLauncher.Views;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WorkspaceLauncher.ViewModels
{
	public class ShellViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<Profile> Profiles
		{
			get
			{
				return new ObservableCollection<Profile>(Configuration.Instance.Profiles);
			}
		}

		private Profile _selectedProfile;
		private ShellView _mainWindow;

		public ShellViewModel()
		{
			SelectedProfile = null;
			_mainWindow = new ShellView();
			_mainWindow.Topmost = Configuration.Instance.AlwaysOnTop;
			_mainWindow.DataContext = this;
			_mainWindow.Show();
			if (Configuration.Instance.LaunchProfileId != 0)
			{
				SelectedProfile = Configuration.Instance.ProfileById(Configuration.Instance.LaunchProfileId);
				_launchAndMove(null);
			}
			GithubUpdater updater = new GithubUpdater();
			updater.LaunchUpdaterAsync();
		}

		public Profile SelectedProfile
		{
			get { return _selectedProfile; }
			set
			{
				_selectedProfile = value;
				if (value != null)
				{
					Configuration.Instance.LastOpenProfileId = value.Id;

				}
				OnPropertyChanged("SelectedProfile");
			}
		}

		public Command SetProgramsCommand { get { return new Command(_setPrograms); } }

		private void _setPrograms(object parameter)
		{
			if (SelectedProfile != null)
			{
				SelectProcessesViewModel SelectProcessesDialog = new SelectProcessesViewModel(SelectedProfile.Id);
			}
		}

		public Command AddProfileCommand { get { return new Command(_addProfile); } }
		private void _addProfile(object parameter)
		{
			ReturnStringDialogViewModel ProfileNamer = new ReturnStringDialogViewModel("Add new profile", "Please chooose a unique name for your new profile");
			if (ProfileNamer.DialogResult == 1)
			{
				int id = Profile.NextId();
				if(Configuration.Instance.Profiles == null)
				{
					Configuration.Instance.Profiles = new List<Profile>();
				}
				Configuration.Instance.Profiles.Add(new Profile() { Id = id, Name = ProfileNamer.Value, Programs = new List<WindowsProgram>() });
				Configuration.Save();
				OnPropertyChanged("Profiles");
				SelectedProfile = Configuration.Instance.ProfileById(id);
			}
		}

		public Command RenameProfileCommand { get { return new Command(_renameProfile); } }
		private void _renameProfile(object parameter)
		{
			ReturnStringDialogViewModel ProfileNamer = new ReturnStringDialogViewModel("Rename profile", "Please chooose a new name for profile: " + SelectedProfile);
			if (ProfileNamer.DialogResult == 1)
			{
				SelectedProfile.Name = ProfileNamer.Value;
			}
		}

		public Command DeleteProfileCommand { get { return new Command(_deleteProfile); } }
		private void _deleteProfile(object parameter)
		{
			if (SelectedProfile != null)
			{
				ConfirmationDialogViewModel ConfirmationBox = new ConfirmationDialogViewModel("Delete Profile", "Are you sure you want to delete " + SelectedProfile + "?");
				if (ConfirmationBox.DialogResult == 1)
				{
					Configuration.Instance.Profiles.Remove(SelectedProfile);
					OnPropertyChanged("Profiles");
					Configuration.Save();
				}
			}
		}

		public Command LaunchCommand { get { return new Command(Launch); } }
		private void Launch(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAll(SelectedProfile.Programs);
			}
		}

		public Command SettingsCommand { get { return new Command(_settings); } }
		private void _settings(object parameter)
		{
			SettingsViewModel SettingsVM = new SettingsViewModel();
			SelectedProfile = null;
			_mainWindow.Topmost = Configuration.Instance.AlwaysOnTop;
		}

		public Command LaunchMoveCommand { get { return new Command(_launchAndMove); } }
		private void _launchAndMove(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAndPositionAll(SelectedProfile.Programs);
			}
		}

		public Command MoveCommand { get { return new Command(_move); } }
		private void _move(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.PositionAll(SelectedProfile.Programs);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
