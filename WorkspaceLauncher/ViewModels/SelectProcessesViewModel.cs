using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Models;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class SelectProcessesViewModel : INotifyPropertyChanged
	{
		private int _selectedProfileId;
		private List<Process> _openWindows;
		private WindowsProgram _selectedProgram;

		public SelectProcessesViewModel(int selectedProfileId)
		{
			SelectedProcesses = new List<Process>();
			this.SelectedProfileId = selectedProfileId;
			_refreshOpenWindows(null);
			SelectProcessesView NewDialog = new SelectProcessesView();
			NewDialog.Topmost = Configuration.Instance.AlwaysOnTop;
			NewDialog.DataContext = this;
			NewDialog.ShowDialog();
		}

		public ObservableCollection<WindowsProgram> ProfilePrograms
		{
			get
			{
				return new ObservableCollection<WindowsProgram>(Configuration.Instance.ProfileById(SelectedProfileId).Programs);
			}
		}

		public List<Process> SelectedProcesses { get; set; }

		public WindowsProgram SelectedProgram
		{
			get { return _selectedProgram; }
			set
			{
				_selectedProgram = value;
				OnPropertyChanged("SelectedProgram");
			}
		}

		public int SelectedProfileId
		{
			get { return _selectedProfileId; }
			set
			{
				_selectedProfileId = value;
				OnPropertyChanged("SelectedProfileId");
			}
		}

		public List<Process> OpenWindows
		{
			get { return _openWindows; }
			set
			{
				_openWindows = value;
				OnPropertyChanged("OpenWindows");
			}
		}

		public Command RefreshOpenWindowsCommand { get { return new Command(_refreshOpenWindows); } }
		private void _refreshOpenWindows(object parameter)
		{
			OpenWindows = new List<Process>();
			foreach (Process Proc in Process.GetProcesses())
			{
				if (!string.IsNullOrEmpty(Proc.MainWindowTitle))
				{
					OpenWindows.Add(Proc);
				}
			}
		}

		public Command SaveProcessesCommand { get { return new Command(_saveProcesses); } }
		private void _saveProcesses(object parameter)
		{
			if(SelectedProcesses.Count > 0)
			{
				foreach (Process aProc in SelectedProcesses)
				{
					WindowsProgram addProgram = new WindowsProgram() { Id = WindowsProgram.NextId(SelectedProfileId) };
					addProgram.StartPath = aProc.MainModule.FileName;
					addProgram.WindowWidth = WindowController.GetWindowWidth(aProc);
					addProgram.WindowHeight = WindowController.GetWindowHeight(aProc);
					addProgram.XPos = WindowController.WindowXPosition(aProc);
					addProgram.YPos = WindowController.WindowYPosition(aProc);
					addProgram.WindowState = WindowController.GetWindowStatus(aProc);
					addProgram.ProcessName = aProc.ProcessName;

					if (aProc.ProcessName == "chrome")
					{
						ReturnStringDialogViewModel ArgumentSetter = new ReturnStringDialogViewModel("Chrome website", "Which website should be opened when chrome is launched?");
						if(ArgumentSetter.DialogResult == 1)
						{
							addProgram.Argument = ArgumentSetter.Value;
						}
					}

					Configuration.Instance.Profiles.SingleOrDefault(x => x.Id == SelectedProfileId).Programs.Add(addProgram);
					Configuration.Save();
				}
				OnPropertyChanged("ProfilePrograms");
			}
		}

		public Command ClearProcessesCommand { get { return new Command(_clearProcesses); } }
		private void _clearProcesses(object parameter)
		{
			Configuration.Instance.Profiles.SingleOrDefault(x => x.Id == SelectedProfileId).Programs = new List<WindowsProgram>();
			Configuration.Save();
			OnPropertyChanged("ProfilePrograms");
		}

		public Command RemoveSelectedProgramCommand { get { return new Command(_removeSelectedProgram); } }
		private void _removeSelectedProgram(object parameter)
		{
			if(SelectedProgram != null)
			{
				Configuration.Instance.Profiles.SingleOrDefault(x => x.Id == SelectedProfileId).Programs.Remove(Configuration.Instance.Profiles.SingleOrDefault(x => x.Id == SelectedProfileId).Programs.SingleOrDefault(x => x.Id == SelectedProgram.Id));
			}
			OnPropertyChanged("ProfilePrograms");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string PropertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
	}
}
