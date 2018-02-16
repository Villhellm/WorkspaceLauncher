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

		public string SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; OnPropertyChanged("SelectedProfile"); }
		}

		public List<WindowsProgram> ProfilePrograms
		{
			get { return Configuration.Programs(SelectedProfile); }
		}

		private List<Process> _openWindows;

		public List<Process> OpenWindows
		{
			get { return _openWindows; }
			set { _openWindows = value; OnPropertyChanged("OpenWindows"); }
		}

		public List<Process> SelectedProcesses { get; set; }

		public SelectProcessesViewModel(string SelectedProfile)
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

			foreach(Process AProc in SelectedProcesses)
			{
				WindowsProgram AddProgram = Configuration.AddProgram(SelectedProfile, AProc.ProcessName);
				if(AddProgram != null)
				{
					AddProgram.StartPath = AProc.MainModule.FileName;
					AddProgram.WindowWidth = WindowController.GetWindowWidth(AProc);
					AddProgram.WindowHeight = WindowController.GetWindowHeight(AProc);
					AddProgram.XPos = WindowController.WindowXPosition(AProc);
					AddProgram.YPos = WindowController.WindowYPosition(AProc);
					AddProgram.WindowState = WindowController.GetWindowStatus(AProc);
				}
			}
			OnPropertyChanged("ProfilePrograms");
		}

		public void ClearProcesses()
		{
			foreach(WindowsProgram Prog in Configuration.Programs(SelectedProfile))
			{
				Configuration.RemoveProgram(SelectedProfile, Prog.Id);
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
