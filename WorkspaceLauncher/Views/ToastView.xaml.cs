using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorkspaceLauncher.Views
{
	/// <summary>
	/// Interaction logic for ToastView.xaml
	/// </summary>
	public partial class ToastView : Window
	{
		public ToastView()
		{
			InitializeComponent();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
				Closing -= Window_Closing;
				e.Cancel = true;
				var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(.5));
				anim.Completed += (s, _) => this.Close();
				this.BeginAnimation(UIElement.OpacityProperty, anim);		
		}
	}
}
