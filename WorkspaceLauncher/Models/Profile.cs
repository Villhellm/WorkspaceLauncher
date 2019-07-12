using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkspaceLauncher.Models
{
	public class Profile : INotifyPropertyChanged
	{
		private int _id;
		private string _name;

		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				OnPropertyChanged("Id");
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		public List<WindowsProgram> Programs { get; set; }

		public static int NextId()
		{
			var config = Configuration.Instance;
			if (config.Profiles != null && config.Profiles.Count > 0)
			{
				return config.Profiles.OrderByDescending(x => x.Id).FirstOrDefault().Id++;
			}
			return 1;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string Property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(Property));
			}
		}
	}
}
