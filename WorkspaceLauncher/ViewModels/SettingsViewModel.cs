﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

		public SettingsViewModel(Point StartPosition = new Point())
		{
			Profiles = Configuration.Profiles;
			if (Profiles.Count > 0)
			{
				SelectedProfile = Profiles[0];
			}

			SettingsView SettingsDialog = new SettingsView();
			if (StartPosition != new Point())
			{
				SettingsDialog.Top = StartPosition.Y-SettingsDialog.Height/3;
				SettingsDialog.Left = StartPosition.X;
			}
			SettingsDialog.Topmost = Configuration.AlwaysOnTop;
			SettingsDialog.DataContext = this;
			SettingsDialog.ShowDialog();
		}

		public string Version { get { return Configuration.Version; } }

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
			}
		}

		public List<WindowsProgram> Programs
		{
			get { return _programs; }
			set
			{
				_programs = value;
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

		public ICommand RemoveProgramCommand { get { return new Command(_removeProgram); } }
		private void _removeProgram(object parameter)
		{
			if ((SelectedProgram != null) && (SelectedProfile != null))
			{

				Configuration.RemoveProgram(SelectedProfile, SelectedProgram.Id);
				Programs = Configuration.Programs(SelectedProfile);
				if (Programs.Count > 0)
				{
					SelectedProgram = Programs[0];
				}

			}
		}

		public ICommand CheckForUpdatesCommand { get { return new Command(_checkForUpdates); } }
		private void _checkForUpdates(object parameter)
		{
			GithubUpdater Updater = new GithubUpdater();
			if(Updater.LaunchUpdater() == 0)
			{
				ToastViewModel.Show("No update available");
			}
		}

		public ICommand FindFileCommand { get { return new Command(_findFile); } }
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
