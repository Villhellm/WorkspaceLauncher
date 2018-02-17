
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkspaceLauncher.Models;
using WorkspaceLauncher.Views;


namespace WorkspaceLauncher.ViewModels
{
	public class ShellViewModel : INotifyPropertyChanged
	{
		private List<string> _profiles;
		private string _selectedProfile;
		private ShellView _mainWindow;

		public ShellViewModel()
		{
			Configuration.CreateAndVerifyConfigurationFile();
			Profiles = Configuration.Profiles;
			if (Profiles.Count > 0)
			{
				SelectedProfile = Profiles[0];
			}
			_mainWindow = new ShellView();
			_mainWindow.Topmost = Configuration.AlwaysOnTop;
			_mainWindow.DataContext = this;
			_mainWindow.Show();
			if (Configuration.LaunchProfile != "None")
			{
				SelectedProfile = Configuration.LaunchProfile;
				_launchAndMove(null);
			}
			GithubUpdater updater = new GithubUpdater();
			updater.LaunchUpdaterAsync();
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
				Configuration.LastOpenProfile = value;
				OnPropertyChanged("SelectedProfile");
			}
		}

		public ICommand SetProgramsCommand { get { return new Command(_setPrograms); } }
		private void _setPrograms(object parameter)
		{
			if (SelectedProfile != null)
			{
				int OldSelected = Profiles.IndexOf(SelectedProfile);
				SelectProcessesViewModel SelectProcessesDialog = new SelectProcessesViewModel(SelectedProfile);
				Profiles = Configuration.Profiles;
				SelectedProfile = Profiles[OldSelected];
			}
		}

		public ICommand AddProfileCommand { get { return new Command(_addProfile); } }
		private void _addProfile(object parameter)
		{
			ReturnStringDialogViewModel ProfileNamer = new ReturnStringDialogViewModel("Add new profile","Please chooose a unique name for your new profile");
			if(ProfileNamer.DialogResult == 1)
			{
				Configuration.AddProfile(ProfileNamer.Value);
				Profiles = Configuration.Profiles;
				SelectedProfile = ProfileNamer.Value;
			}
		}

		public ICommand RenameProfileCommand { get { return new Command(_renameProfile); } }
		private void _renameProfile(object parameter)
		{
			ReturnStringDialogViewModel ProfileNamer = new ReturnStringDialogViewModel("Rename profile", "Please chooose a new name profile " + SelectedProfile);
			if (ProfileNamer.DialogResult == 1)
			{
				Configuration.RenameProfile(SelectedProfile, ProfileNamer.Value);
				Profiles = Configuration.Profiles;
				SelectedProfile = Profiles[0];
			}
		}

		public ICommand DeleteProfileCommand { get { return new Command(_deleteProfile); } }
		private void _deleteProfile(object parameter)
		{
			if (SelectedProfile != null)
			{
				ConfirmationDialogViewModel ConfirmationBox = new ConfirmationDialogViewModel("Delete Profile", "Are you sure you want to delete " + SelectedProfile + "?");
				if(ConfirmationBox.DialogResult == 1)
				{
					Configuration.DeleteProfile(SelectedProfile);
					Profiles = Configuration.Profiles;
				}
			}
		}

		public ICommand LaunchCommand { get { return new Command(Launch); } }
		private void Launch(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAll(Configuration.Programs(SelectedProfile));
			}
		}

		public ICommand SettingsCommand { get { return new Command(_settings); } }
		private void _settings(object parameter)
		{
			SettingsViewModel SettingsVM = new SettingsViewModel();
			_mainWindow.Topmost = Configuration.AlwaysOnTop;
		}

		public ICommand LaunchMoveCommand { get { return new Command(_launchAndMove); } }
		private void _launchAndMove(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAndPositionAll(Configuration.Programs(SelectedProfile));
			}
		}

		public ICommand MoveCommand { get { return new Command(_move); } }
		private void _move(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.PositionAll(Configuration.Programs(SelectedProfile));
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
