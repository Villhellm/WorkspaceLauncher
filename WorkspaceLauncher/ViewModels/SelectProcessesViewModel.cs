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
		private Profile _selectedProfile;

		public Profile SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; OnPropertyChanged("SelectedProfile"); }
		}

		private List<Process> _openWindows;

		public List<Process> OpenWindows
		{
			get { return _openWindows; }
			set { _openWindows = value; OnPropertyChanged("OpenWindows"); }
		}

		public List<Process> SelectedProcesses { get; set; }

		public SelectProcessesViewModel(Profile SelectedProfile)
		{
			this.SelectedProfile = SelectedProfile;
			RefreshOpenWindows();
			SelectProcessesView NewDialog = new SelectProcessesView();
			NewDialog.DataContext = this;
			NewDialog.ShowDialog();
		}

		public void RefreshOpenWindows()
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

		public void SaveProcesses()
		{
			string ProcessName;
			string ProgramStartPath;
			int WindowWidth;
			int WindowHeight;
			int WindowXPos;
			int WindowYPos;
			int WindowStatus;

			foreach(Process AProc in SelectedProcesses)
			{
				ProcessName = AProc.ProcessName;
				ProgramStartPath= AProc.MainModule.FileName;
				WindowWidth = WindowController.GetWindowWidth(AProc);
				WindowHeight = WindowController.GetWindowHeight(AProc);
				WindowXPos = WindowController.WindowXPosition(AProc);
				WindowYPos = WindowController.WindowYPosition(AProc);
				WindowStatus = WindowController.GetWindowStatus(AProc);
				WindowsProgram ProgramToAdd = new WindowsProgram(ProcessName, ProgramStartPath, "", WindowWidth, WindowHeight, WindowXPos, WindowYPos, WindowStatus);
				Configuration.AddProgram(SelectedProfile.ProfileName, ProgramToAdd);
			}
			SelectedProfile = Configuration.Profile(SelectedProfile.ProfileName);
		}

		public void ClearProcesses()
		{
			foreach(WindowsProgram Prog in SelectedProfile.Programs)
			{
				Configuration.RemoveProgram(SelectedProfile.ProfileName, Prog.ProcessName);
			}
			SelectedProfile = Configuration.Profile(SelectedProfile.ProfileName);

		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string PropertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
	}
}
