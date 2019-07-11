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
						//write string to file
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

		private int launchProfileId;

		public int LaunchProfileId
		{
			get { return launchProfileId; }
			set
			{
				launchProfileId = value;
				OnPropertyChanged("LaunchProfileId");
			}
		}


		private decimal version;
		public decimal Version
		{
			get { return version; }
			set
			{
				version = value;
				OnPropertyChanged("Version");
			}
		}

		private int lastOpenProfileId;
		public int LastOpenProfileId
		{
			get { return lastOpenProfileId; }
			set
			{
				lastOpenProfileId = value;
				OnPropertyChanged("LastOpenProfileId");
			}
		}

		private bool checkForUpdates;
		public bool CheckForUpdates
		{
			get { return checkForUpdates; }
			set
			{
				checkForUpdates = value;
				OnPropertyChanged("CheckForUpdates");
			}
		}

		private bool alwaysOnTop;
		public bool AlwaysOnTop
		{
			get { return alwaysOnTop; }
			set
			{
				alwaysOnTop = value;
				OnPropertyChanged("AlwaysOnTop");
			}
		}

		public List<Profile> Profiles;

		public static void Save()
		{
			string json = JsonConvert.SerializeObject(Instance);
			File.WriteAllText(ConfigurationFile, json);
		}

		public Profile ProfileById(int id)
		{
			if(Instance.Profiles.Any(x=>x.Id == id))
			{
				return Instance.Profiles.SingleOrDefault(x => x.Id == id);
			}
			return null;
		}

		public WindowsProgram ProgramById(int profileId, int programId)
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
