using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkspaceLauncher.Models;
using WorkspaceLauncher.Views;

namespace WorkspaceLauncher.ViewModels
{
	public class SelectProcessesViewModel : INotifyPropertyChanged
	{
		private string _selectedProfile;
		private List<Process> _openWindows;
		private WindowsProgram _selectedProgram;

		public SelectProcessesViewModel(string SelectedProfile)
		{
			this.SelectedProfile = SelectedProfile;
			_refreshOpenWindows(null);
			SelectProcessesView NewDialog = new SelectProcessesView();
			NewDialog.Topmost = Configuration.AlwaysOnTop;
			NewDialog.DataContext = this;
			NewDialog.ShowDialog();
		}

		public List<WindowsProgram> ProfilePrograms { get { return Configuration.Programs(SelectedProfile); } }

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

		public string SelectedProfile
		{
			get { return _selectedProfile; }
			set
			{
				_selectedProfile = value;
				OnPropertyChanged("SelectedProfile");
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

		public ICommand RefreshOpenWindowsCommand { get { return new Command(_refreshOpenWindows); } }
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

		public ICommand SaveProcessesCommand { get { return new Command(_saveProcesses); } }
		private void _saveProcesses(object parameter)
		{
			if(SelectedProcesses.Count > 0)
			{
				foreach (Process AProc in SelectedProcesses)
				{
					WindowsProgram AddProgram = Configuration.AddProgram(SelectedProfile, AProc.ProcessName);
					if (AddProgram != null)
					{
						AddProgram.StartPath = AProc.MainModule.FileName;
						AddProgram.WindowWidth = WindowController.GetWindowWidth(AProc);
						AddProgram.WindowHeight = WindowController.GetWindowHeight(AProc);
						AddProgram.XPos = WindowController.WindowXPosition(AProc);
						AddProgram.YPos = WindowController.WindowYPosition(AProc);
						AddProgram.WindowState = WindowController.GetWindowStatus(AProc);
					}
					if (AProc.ProcessName == "chrome")
					{
						ReturnStringDialogViewModel ArgumentSetter = new ReturnStringDialogViewModel("Chrome website", "Which website should be opened when chrome is launched?");
						if(ArgumentSetter.DialogResult == 1)
						{
							AddProgram.Argument = ArgumentSetter.Value;
						}
					}
				}
				OnPropertyChanged("ProfilePrograms");
			}
		}

		public ICommand ClearProcessesCommand { get { return new Command(_clearProcesses); } }
		private void _clearProcesses(object parameter)
		{
			foreach (WindowsProgram Prog in Configuration.Programs(SelectedProfile))
			{
				Configuration.RemoveProgram(SelectedProfile, Prog.Id);
			}
			OnPropertyChanged("ProfilePrograms");
		}

		public ICommand RemoveSelectedProgramCommand { get { return new Command(_removeSelectedProgram); } }
		private void _removeSelectedProgram(object parameter)
		{
			if(SelectedProgram != null)
			{
				Configuration.RemoveProgram(SelectedProfile, SelectedProgram.Id);
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
