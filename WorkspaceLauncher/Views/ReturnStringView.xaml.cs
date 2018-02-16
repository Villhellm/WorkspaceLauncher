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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorkspaceLauncher.ViewModels;

namespace WorkspaceLauncher.Views
{
    /// <summary>
    /// Interaction logic for ReturnStringView.xaml
    /// </summary>
    public partial class ReturnStringView : Window
    {
        public ReturnStringView()
        {
            InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ReturnStringDialogViewModel Current = (ReturnStringDialogViewModel)DataContext;
			Current.Close(1);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			ReturnStringDialogViewModel Current = (ReturnStringDialogViewModel)DataContext;
			Current.Close(0);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			var point = System.Windows.Forms.Control.MousePosition;
			Left = point.X - Width/2;
			Top = point.Y - Height/2;
		}
	}
}
