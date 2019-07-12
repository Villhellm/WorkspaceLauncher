using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkspaceLauncher.Models
{
	public class Configuration : INotifyPropertyChanged
	{
		public static string AppDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WorkspaceLauncher";
		public static string ConfigurationFile = AppDataRoamingPath + @"\configuration.json";

		private int _launchProfileId;
		private decimal _version;
		private int _lastOpenProfileId;
		private bool _checkForUpdates;
		private bool _alwaysOnTop;

		private static Configuration instance = null;
		private Configuration() { Profiles = new List<Profile>(); }
		public static Configuration Instance
		{
			get
			{
				if (instance == null)
				{
					if (!File.Exists(ConfigurationFile))
					{
						Directory.CreateDirectory(AppDataRoamingPath);
						string json = JsonConvert.SerializeObject(new Configuration());
						File.WriteAllText(ConfigurationFile, json);
						instance = new Configuration();
					}
					else
					{
						instance = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationFile));
					}
				}
				return instance;

			}
		}

		public List<Profile> Profiles;

		public int LaunchProfileId
		{
			get { return _launchProfileId; }
			set
			{
				_launchProfileId = value;
				OnPropertyChanged("LaunchProfileId");
			}
		}

		public decimal Version
		{
			get { return _version; }
			set
			{
				_version = value;
				OnPropertyChanged("Version");
			}
		}

		public int LastOpenProfileId
		{
			get { return _lastOpenProfileId; }
			set
			{
				_lastOpenProfileId = value;
				OnPropertyChanged("LastOpenProfileId");
			}
		}

		public bool CheckForUpdates
		{
			get { return _checkForUpdates; }
			set
			{
				_checkForUpdates = value;
				OnPropertyChanged("CheckForUpdates");
			}
		}

		public bool AlwaysOnTop
		{
			get { return _alwaysOnTop; }
			set
			{
				_alwaysOnTop = value;
				OnPropertyChanged("AlwaysOnTop");
			}
		}

		public static void Save()
		{
			string json = JsonConvert.SerializeObject(Instance);
			File.WriteAllText(ConfigurationFile, json);
		}

		public static Profile ProfileById(int id)
		{
			if(Instance.Profiles.Any(x=>x.Id == id))
			{
				return Instance.Profiles.SingleOrDefault(x => x.Id == id);
			}
			return null;
		}

		public static WindowsProgram ProgramById(int profileId, int programId)
		{
			var profile = ProfileById(profileId);
			if (profile != null)
			{
				if(profile.Programs.Any(x=>x.Id == programId))
				{
					return profile.Programs.SingleOrDefault(x => x.Id == programId);
				}
			}
			return null;
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
