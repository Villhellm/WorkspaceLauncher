using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		private Configuration CurrentConfiguration; 

		public ShellViewModel()
		{

		}
	}
}
