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

		public List<string> Profiles
		{
			get { return _profiles; }
			set { _profiles = value; OnPropertyChanged("Profiles"); }
		}

		private string _selectedProfile;

		public string SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; Configuration.LastOpenProfile = value; OnPropertyChanged("SelectedProfile"); }
		}

		ShellView MainWindow;

		public ShellViewModel()
		{
			Configuration.CreateAndVerifyConfigurationFile();
			Profiles = Configuration.Profiles;
			if(Profiles.Count > 0)
			{
				SelectedProfile = Profiles[0];
			}
			MainWindow = new ShellView();
			MainWindow.Topmost = Configuration.AlwaysOnTop;
			MainWindow.DataContext = this;
			MainWindow.Show();
			if(Configuration.LaunchProfile != "None")
			{
				SelectedProfile = Configuration.LaunchProfile;
				LaunchAndMove(null);
			}
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

		public ICommand AddProfileCommand { get { return new Command(AddProfile); } }
		public void AddProfile(object parameter)
		{
			ReturnStringDialogViewModel ProfileNamer = new ReturnStringDialogViewModel("Add new profile","Please chooose a unique name for your new profile");
			if(ProfileNamer.DialogResult == 1)
			{
				Configuration.AddProfile(ProfileNamer.Value);
				Profiles = Configuration.Profiles;
			}
		}

		public ICommand RenameProfileCommand { get { return new Command(RenameProfile); } }
		public void RenameProfile(object parameter)
		{
			ReturnStringDialogViewModel ProfileNamer = new ReturnStringDialogViewModel("Rename profile", "Please chooose a new name profile " + SelectedProfile);
			if (ProfileNamer.DialogResult == 1)
			{
				Configuration.RenameProfile(SelectedProfile, ProfileNamer.Value);
				Profiles = Configuration.Profiles;
			}
		}


		public ICommand DeleteProfileCommand { get { return new Command(DeleteProfile); } }
		public void DeleteProfile(object parameter)
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
			MainWindow.Topmost = Configuration.AlwaysOnTop;
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
