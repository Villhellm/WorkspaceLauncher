using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorkspaceLauncher.ViewModels;

namespace WorkspaceLauncher.Views
{
	/// <summary>
	/// Interaction logic for SelectProcessesView.xaml
	/// </summary>
	public partial class SelectProcessesView : Window
	{
		public SelectProcessesView()
		{
			InitializeComponent();
		}

		private void OpenProcesses_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectProcessesViewModel Current = (SelectProcessesViewModel)DataContext;
			Current.SelectedProcesses = new List<Process>();
			foreach (var item in OpenProcesses.SelectedItems)
			{
				Current.SelectedProcesses.Add((Process)item);
			}
		}

		private void OpenProcesses_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			SelectProcessesViewModel Current = (SelectProcessesViewModel)DataContext;
			Current.SelectedProcesses = new List<Process>();
			var itemIndex = OpenProcesses.SelectedItems.Count - 1;
			var item = OpenProcesses.SelectedItems[itemIndex];
			OpenProcesses.SelectedItems.Clear();
			OpenProcesses.SelectedItems.Add(item);
		}
	}
}
