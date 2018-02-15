using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkspaceLauncher.Models;

namespace WorkspaceLauncher.ViewModels
{
	public class ShellViewModel
	{
		private List<Profile> _profiles;

		public List<Profile> Profiles
		{
			get { return _profiles; }
			set { _profiles = value; }
		}

		private Profile _selectedProfile;

		public Profile SelectedProfile
		{
			get { return _selectedProfile; }
			set { _selectedProfile = value; }
		}


		public ShellViewModel()
		{
			Configuration.CreateAndVerifyConfigurationFile();
			Profiles = Configuration.Profiles;
		}
	}
}
