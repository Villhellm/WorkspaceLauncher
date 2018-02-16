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
		private List<string> _profiles;

		public List<string> Profiles
		{
			get { return _profiles; }
			set { _profiles = value; OnPropertyChanged("Profiles"); }
		}

		private string _selectedProfile;

		public string SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; OnPropertyChanged("SelectedProfile"); }
		}

		public ShellViewModel()
		{
			Configuration.CreateAndVerifyConfigurationFile();
			Profiles = Configuration.Profiles;
			SelectedProfile = Profiles[0];
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
				WindowController.LaunchAll(Configuration.Programs(SelectedProfile));
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
				WindowController.LaunchAndPositionAll(Configuration.Programs(SelectedProfile));
			}
		}

		public ICommand MoveCommand { get { return new Command(Move); } }
		public void Move(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.PositionAll(Configuration.Programs(SelectedProfile));
			}
		}

		public ICommand DeleteProfileCommand { get { return new Command(DeleteProfile); } }
		public void DeleteProfile(object parameter)
		{
			if (SelectedProfile != null)
			{
				Configuration.DeleteProfile(SelectedProfile);
				Profiles = Configuration.Profiles;
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
