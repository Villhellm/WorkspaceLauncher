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
			set { _profiles = value; }
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

		public ICommand LaunchCommand { get { return new Command(Launch); } }
		public ICommand LaunchMoveCommand { get { return new Command(LaunchAndMove); } }
		public ICommand MoveCommand { get { return new Command(Move); } }
		public ICommand SetProgramsCommand { get; }

		public void Launch(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAll(SelectedProfile.Programs);
			}
		}
		public void LaunchAndMove(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.LaunchAndPositionAll(SelectedProfile.Programs);
			}
		}
		public void Move(object parameter)
		{
			if (SelectedProfile != null)
			{
				WindowController.PositionAll(SelectedProfile.Programs);
			}
		}
	}
}
