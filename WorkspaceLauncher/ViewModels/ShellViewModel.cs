using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkspaceLauncher.Models;

namespace WorkspaceLauncher.ViewModels
{
	public class ShellViewModel : INotifyPropertyChanged
	{
		private List<Profile> _profiles;

		public List<Profile> Profiles
		{
			get { return _profiles; }
			set { _profiles = value; OnPropertyChanged("Profiles"); }
		}

		private Profile _selectedProfile;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string Property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(Property));
			}
		}

		public Profile SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; OnPropertyChanged("SelectedProfile"); }
		}

		public ShellViewModel()
		{
			Configuration.CreateAndVerifyConfigurationFile();
			Profiles = Configuration.Profiles;
		}

		public ICommand SetProgramsCommand { get { return new Command(SetPrograms); } }
		public void SetPrograms(object parameter)
		{
			if (SelectedProfile != null)
			{
				int OldSelected = Profiles.IndexOf(SelectedProfile);
				SelectProcessesViewModel SelectProcessesDialog = new SelectProcessesViewModel(SelectedProfile);
				Profiles = Configuration.Profiles;
				SelectedProfile = Profiles[OldSelected];
			}
		}

		public ICommand LaunchCommand { get { return new Command(Launch); } }
		public void Launch(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAll(SelectedProfile.Programs);
			}
		}

		public ICommand SettingsCommand { get { return new Command(Settings); } }
		public void Settings(object parameter)
		{
			SettingsViewModel SettingsVM = new SettingsViewModel();
		}

		public ICommand LaunchMoveCommand { get { return new Command(LaunchAndMove); } }
		public void LaunchAndMove(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAndPositionAll(SelectedProfile.Programs);
			}
		}

		public ICommand MoveCommand { get { return new Command(Move); } }
		public void Move(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.PositionAll(SelectedProfile.Programs);
			}
		}

		public ICommand DeleteProfileCommand { get { return new Command(DeleteProfile); } }
		public void DeleteProfile(object parameter)
		{
			if (SelectedProfile != null)
			{
				Configuration.DeleteProfile(SelectedProfile.ProfileName);
				Profiles = Configuration.Profiles;
			}
		}
	}
}
