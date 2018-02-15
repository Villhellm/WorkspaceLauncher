using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkspaceLauncher.Models
{
	public class Profile
	{
		public string ProfileName { get; set; }
		public List<WindowsProgram> Programs { get; set; }

		public Profile(string ProfileName, List<WindowsProgram> Programs)
		{
			this.ProfileName = ProfileName;
			this.Programs = Programs;
		}
	}
}
