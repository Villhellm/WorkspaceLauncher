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
	public class Configuration
	{
		public static string AppDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WorkspaceLauncher";
		public static string ConfigurationFile = AppDataRoamingPath + @"\Configuration.xml";

		public string Version { get; set; }
		public string LastProfileOpen { get; set; }
		public string LaunchProfile { get; set; }
		public bool AlwaysOnTop { get; set; }
		public bool CheckForUpdates { get; set; }
		public List<Profile> Profiles { get; set; }

		public Profile Profile(string ProfileName)
		{
			return Profiles.Where(x => x.ProfileName == ProfileName).FirstOrDefault();
		}

		public WindowsProgram Program(string ProfileName, string ProgramName)
		{
			return Profile(ProfileName).Programs.Where(x => x.ProcessName == ProgramName).FirstOrDefault();
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
						new XElement("CheckForUpdates","true"),
						new XElement("LastProfileOpen"),
						new XElement("Profiles",
							new XElement("Profile",
								new XElement ("Name", "Default"),
								new XElement ("Programs")
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

		public static Configuration Load()
		{
			CreateConfigurationFile();
			VerifyConfigurationIntegrity();
			XDocument Xml = XDocument.Load(ConfigurationFile);
			XElement xUpdates = Xml.Element("Configs").Element("CheckForUpdates");

			XElement xAlwaysOnTop = Xml.Element("Configs").Element("AlwaysOnTop");

			XElement xCurrentVersion = Xml.Element("Configs").Element("Version");

			XElement xLaunchProfile = Xml.Element("Configs").Element("LaunchProfile");

			XElement xLastOpen = Xml.Element("Configs").Element("LastOpenProfile");
			List<Profile> ProfileList = new List<Profile>();
			List<WindowsProgram> ProgramList;

			string ProcessName = "";
			string ProgramStartPath = "";
			string ProgramArgument = "";
			int WindowWidth = 0;
			int WindowHeight = 0;
			int WindowXPos = 0;
			int WindowYPos = 0;
			int WindowStatus;

			IEnumerable<XElement> xProfiles = Xml.Element("Configs").Element("Profiles").Elements("Profile");
			foreach (XElement xProfile in xProfiles)
			{

				ProgramList = new List<WindowsProgram>();
				IEnumerable<XElement> xPrograms = xProfile.Element("Programs").Elements("Program");
				foreach (XElement xProgram in xPrograms)
				{
					ProcessName = xProgram.Element("ProcessName").Value;
					ProgramStartPath = xProgram.Element("StartPath").Value;
					ProgramArgument = xProgram.Element("Argument").Value;
					WindowWidth = Convert.ToInt32(xProgram.Element("WindowWidth").Value);
					WindowHeight = Convert.ToInt32(xProgram.Element("WindowHeight").Value);
					WindowXPos = Convert.ToInt32(xProgram.Element("WindowXPos").Value);
					WindowYPos = Convert.ToInt32(xProgram.Element("WindowYPos").Value);
					WindowStatus = Convert.ToInt32(xProgram.Element("WindowState").Value);
					ProgramList.Add(new WindowsProgram(ProcessName, ProgramStartPath, ProgramArgument, WindowWidth, WindowHeight, WindowXPos, WindowYPos, WindowStatus));
				}
				ProfileList.Add(new Profile(xProfile.Element("Name").Value.ToString(), ProgramList));
			}

			Configuration LoadedConfiguration = new Configuration();
			LoadedConfiguration.AlwaysOnTop = Convert.ToBoolean(xAlwaysOnTop.Value);
			LoadedConfiguration.CheckForUpdates = Convert.ToBoolean(xUpdates.Value);
			LoadedConfiguration.LastProfileOpen = xLastOpen.Value.ToString();
			LoadedConfiguration.Version = xCurrentVersion.Value.ToString();
			LoadedConfiguration.Profiles = ProfileList;
			LoadedConfiguration.LaunchProfile = xLaunchProfile.Value;

			return LoadedConfiguration;
		}

		public void RemoveProgram(string ProfileName, string ProgramName)
		{
			Profile(ProfileName).Programs.Remove(Program(ProfileName, ProgramName));
		}

		public void Save()
		{
			XDocument Xml = XDocument.Load(ConfigurationFile);
			XElement xRoot = Xml.Element("Configs");
			Xml.Save(ConfigurationFile);
			xRoot.Element("Profiles").Elements("Profile").Remove();
			foreach (Profile wProfile in Profiles)
			{
				AddProfileNode(Xml, wProfile.ProfileName);
				XElement xProfile = xRoot.Element("Profiles").Elements("Profile").Where(x => x.Element("Name").Value == wProfile.ProfileName).First();
				xProfile.Element("Programs").Elements("Program").Remove();
				foreach (WindowsProgram wProgram in wProfile.Programs)
				{
					AddProgramNode(Xml, wProfile.ProfileName, wProgram.ProcessName, wProgram.StartPath, wProgram.Argument, wProgram.WindowWidth, wProgram.WindowHeight, wProgram.XPos, wProgram.YPos, wProgram.Status);
				}
			}
			Xml.Element("Configs").Element("AlwaysOnTop").Value = AlwaysOnTop.ToString();
			Xml.Element("Configs").Element("CheckForUpdates").Value = CheckForUpdates.ToString();
			Xml.Element("Configs").Element("Version").Value = Version;
			Xml.Element("Configs").Element("LastOpenProfile").Value = LastProfileOpen;
			Xml.Element("Configs").Element("LaunchProfile").Value = LaunchProfile;
			Xml.Save(ConfigurationFile);
		}

		private void AddProfileNode(XDocument Xml, string ProfileName)
		{
			Xml.Element("Configs").Element("Profiles").Add(new XElement("Profile", new XElement("Name", ProfileName), new XElement("Programs")));
		}

		private void AddProgramNode(XDocument Xml, string Profile, string ProgName, string ProgPath, string ProgArg, int WindowWidth, int WindowHeight, int xPos, int yPos, int Status)
		{
			XElement xProfile = Xml.Element("Configs").Element("Profiles").Elements("Profile").Single(x => (string)x.Element("Name") == Profile).Element("Programs");
			xProfile.Add(new XElement("Program", new XElement("ProcessName", ProgName), new XElement("StartPath", ProgPath), new XElement("Argument", ProgArg), new XElement("WindowHeight", WindowHeight), new XElement("WindowWidth", WindowWidth), new XElement("WindowXPos", xPos), new XElement("WindowYPos", yPos), new XElement("WindowState", Status)));
		}

		public void AddNewProfile(Profile NewProfile)
		{
			Profiles.Add(NewProfile);
		}

		public void DeleteProfile(string ProfileName)
		{
			Profiles.Remove(Profile(ProfileName));
		}

		public int RenameProfile(string OldName, string NewName)
		{
			Profile SelectedProfile = Profiles.Where(x => x.ProfileName == OldName).FirstOrDefault();
			Profile CheckIfExist = Profiles.Where(x => x.ProfileName == NewName).FirstOrDefault();
			if (CheckIfExist == null)
			{
				Profile(OldName).ProfileName = NewName;
				return 1;
			}
			else
			{
				return 0;
			}
		}
	}
}
