using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.Models;

namespace WorkspaceLauncher
{
	public static class Configuration
	{
		public static string AppDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WorkspaceLauncher";
		public static string ConfigurationFile = AppDataRoamingPath + @"\Configuration.xml";

		public static XDocument xConfiguration { get { return XDocument.Load(ConfigurationFile); } }

		public static List<string> Profiles
		{
			get
			{
				List<string> ReturnList = new List<string>();

				IEnumerable<XElement> xProfiles = xConfiguration.Element("Configs").Element("Profiles").Elements("Profile");
				foreach (XElement xProfile in xProfiles)
				{
					ReturnList.Add(xProfile.Element("Name").Value);
				}
				return ReturnList;
			}
		}

		public static string Version
		{
			get { return xConfiguration.Element("Configs").Element("Version").Value; }
			set
			{
				XDocument xConfig = xConfiguration;
				xConfig.Element("Configs").Element("Version").Value = value;
				xConfig.Save(ConfigurationFile);
			}
		}

		public static string LastOpenProfile
		{
			get { return xConfiguration.Element("Configs").Element("LastOpenProfile").Value; }
			set
			{
				XDocument xConfig = xConfiguration;
				if(value == null)
				{
					xConfig.Element("Configs").Element("LastOpenProfile").Value = "None";
				}
				else
				{
					xConfig.Element("Configs").Element("LastOpenProfile").Value = value;
				}
				xConfig.Save(ConfigurationFile);
			}
		}

		public static string LaunchProfile
		{
			get { return xConfiguration.Element("Configs").Element("LaunchProfile").Value; }
			set
			{
				XDocument xConfig = xConfiguration;
				xConfig.Element("Configs").Element("LaunchProfile").Value = value;
				xConfig.Save(ConfigurationFile);
			}
		}

		public static bool AlwaysOnTop
		{
			get { return Convert.ToBoolean(xConfiguration.Element("Configs").Element("AlwaysOnTop").Value); }
			set
			{
				XDocument xConfig = xConfiguration;
				xConfig.Element("Configs").Element("AlwaysOnTop").Value = value.ToString();
				xConfig.Save(ConfigurationFile);
			}
		}

		public static bool CheckForUpdates
		{
			get { return Convert.ToBoolean(xConfiguration.Element("Configs").Element("CheckForUpdates").Value); }
			set
			{
				XDocument xConfig = xConfiguration;
				xConfig.Element("Configs").Element("CheckForUpdates").Value = value.ToString();
				xConfig.Save(ConfigurationFile);
			}
		}

		public static List<WindowsProgram> Programs(string ProfileName)
		{
			List<WindowsProgram> ReturnList = new List<WindowsProgram>();
			foreach (XElement Program in xConfiguration.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == ProfileName).First().Element("Programs").Elements("Program"))
			{
				ReturnList.Add(new WindowsProgram(ProfileName, Convert.ToInt32(Program.Element("Id").Value)));
			}
			return ReturnList;
		}

		public static XElement Program(XDocument ParentDocument, string ProfileName, int Id)
		{
			return ParentDocument.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == ProfileName).First().Element("Programs").Elements("Program").Where(x => x.Element("Id").Value == Id.ToString()).First();
		}

		private static int NextId(string ProfileName)
		{
			List<int> Ids = new List<int>();
			Ids.Add(0);
			XElement Profile = xConfiguration.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == ProfileName).First();
			if (xConfiguration.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == ProfileName).First().Element("Programs").Elements("Program").Any())
			{
				foreach (XElement Program in Profile.Element("Programs").Elements("Program"))
				{
					if (Program.Element("Id") != null)
					{
						Ids.Add(Convert.ToInt32(Program.Element("Id").Value));
					}
				}
			}
			Ids.Sort();
			int LatestId = Ids[Ids.Count - 1];
			LatestId++;
			return LatestId;
		}

		public static void CreateAndVerifyConfigurationFile()
		{
			CreateConfigurationFile();
			VerifyConfigurationIntegrity();
		}

		private static void CreateConfigurationFile()
		{
			if (!File.Exists(ConfigurationFile))
			{
				Directory.CreateDirectory(AppDataRoamingPath);
				XDocument NewConfig = new XDocument();
				NewConfig.Add(
					new XElement("Configs",
						new XElement("Version"),
						new XElement("AlwaysOnTop", "false"),
						new XElement("CheckForUpdates", "true"),
						new XElement("LaunchProfile", "None"),
						new XElement("LastProfileOpen"),
						new XElement("Profiles",
							new XElement("Profile",
								new XElement("Name", "Default"),
								new XElement("Programs")
						))
					));
				NewConfig.Save(ConfigurationFile);
			}
		}

		private static void VerifyConfigurationIntegrity()
		{
			XDocument Xml = XDocument.Load(ConfigurationFile);

			XElement ElementToVerify = Xml.Element("Configs");
			if (ElementToVerify == null)
			{
				Xml.Add(ElementToVerify);
				Xml.Save(ConfigurationFile);
			}
			VerifyConfigElement(Xml, "AlwaysOnTop", "false");
			VerifyConfigElement(Xml, "CheckForUpdates", "true");
			VerifyConfigElement(Xml, "Version", "1.0.0.0");
			VerifyConfigElement(Xml, "LastOpenProfile", "");
			VerifyConfigElement(Xml, "Profiles", "");
			VerifyConfigElement(Xml, "LaunchProfile", "None");
			VerifyProfiles(Xml);
		}

		private static void VerifyConfigElement(XDocument Xml, string ElementName, string DefaultValue)
		{
			XElement ElementToVerify = Xml.Element("Configs").Element(ElementName);
			if (ElementToVerify == null)
			{
				Xml.Element("Configs").AddFirst(new XElement(ElementName, DefaultValue));
				Xml.Save(ConfigurationFile);
			}
		}

		private static void VerifyProfiles(XDocument Xml)
		{
			IEnumerable<XElement> ElementsToVerify = Xml.Element("Configs").Element("Profiles").Elements("Profile");
			foreach (XElement Profile in ElementsToVerify)
			{
				if (Profile.Element("Name") == null)
				{
					Profile.Add(new XElement("Name"));
				}
				if (Profile.Element("Programs") == null)
				{
					Profile.Add(new XElement("Programs"));
				}
				VerifyPrograms(Profile);
			}
			Xml.Save(ConfigurationFile);
		}

		public static void VerifyPrograms(XElement Profile)
		{
			IEnumerable<XElement> ElementsToVerify = Profile.Element("Programs").Elements("Program");
			foreach (XElement Program in ElementsToVerify)
			{
				if (Program.Element("Id") == null)
				{
					Program.Add(new XElement("Id", NextId(Profile.Element("Name").Value)));
				}
				if (Program.Element("ProcessName") == null)
				{
					Program.Add(new XElement("ProcessName", 1));
				}
				if (Program.Element("StartPath") == null)
				{
					Program.Add(new XElement("StartPath", 1));
				}
				if (Program.Element("Argument") == null)
				{
					Program.Add(new XElement("Argument", 1));
				}
				if (Program.Element("WindowHeight") == null)
				{
					Program.Add(new XElement("WindowHeight", 1));
				}
				if (Program.Element("WindowWidth") == null)
				{
					Program.Add(new XElement("WindowWidth", 1));
				}
				if (Program.Element("WindowXPos") == null)
				{
					Program.Add(new XElement("WindowXPos", 1));
				}
				if (Program.Element("WindowYPos") == null)
				{
					Program.Add(new XElement("WindowYPos", 1));
				}
				if (Program.Element("WindowState") == null)
				{
					Program.Add(new XElement("WindowState", 1));
				}
			}

		}

		public static void RemoveProgram(string ProfileName, int ProgramId)
		{
			XDocument xConfig = xConfiguration;
			XElement xProfile = xConfig.Element("Configs").Element("Profiles").Elements("Profile").Single(x => (string)x.Element("Name") == ProfileName).Element("Programs");
			xProfile.Elements("Program").Where(x => x.Element("Id").Value == ProgramId.ToString()).Remove();
			xConfig.Save(ConfigurationFile);
		}

		public static WindowsProgram AddProgram(string ProfileName, string NewProgram)
		{
			XDocument xConfig = xConfiguration;
			XElement xProfilePrograms = xConfig.Element("Configs").Element("Profiles").Elements("Profile").Single(x => (string)x.Element("Name") == ProfileName).Element("Programs");
			int ProgramId = NextId(ProfileName);
			xProfilePrograms.Add(new XElement("Program", new XElement("Id", ProgramId), new XElement("ProcessName", NewProgram), new XElement("StartPath"), new XElement("Argument"), new XElement("WindowHeight"), new XElement("WindowWidth"), new XElement("XPos"), new XElement("YPos"), new XElement("WindowState")));
			xConfig.Save(ConfigurationFile);
			return new WindowsProgram(ProfileName, ProgramId);
		}

		public static int AddProfile(string NewProfile)
		{
			XDocument xConfig = xConfiguration;
			if (!xConfig.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == NewProfile).Any())
			{
				xConfig.Element("Configs").Element("Profiles").Add(new XElement("Profile", new XElement("Name", NewProfile), new XElement("Programs")));
				xConfig.Save(ConfigurationFile);
				return 1;
			}
			return 0;
		}

		public static void DeleteProfile(string ProfileName)
		{
			XDocument xConfig = xConfiguration;
			xConfig.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == ProfileName).Remove();
			xConfig.Save(ConfigurationFile);
			if (LaunchProfile == ProfileName)
			{
				LaunchProfile = "None";
			}
		}

		public static int RenameProfile(string OldName, string NewName)
		{
			XDocument xConfig = xConfiguration;
			XElement SelectedProfile = xConfig.Element("Configs").Element("Profiles").Elements("Profile").Single(x => x.Element("Name").Value == OldName);
			if (!xConfig.Element("Configs").Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == NewName).Any())
			{
				SelectedProfile.Element("Name").Value = NewName;
				xConfig.Save(ConfigurationFile);
				if (LaunchProfile == OldName)
				{
					LaunchProfile = NewName;
				}
				return 1;
			}
			else
			{
				return 0;
			}
		}
	}
}
