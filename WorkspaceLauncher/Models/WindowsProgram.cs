using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WorkspaceLauncher.Models
{
	public class WindowsProgram
	{
		public string ParentProfileName { get; set; }
		public int Id { get; set; }

		public string ProcessName
		{
			get
			{
				return Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("ProcessName").Value;
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("ProcessName").Value = value;
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public string StartPath
		{
			get
			{
				return Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("StartPath").Value;
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("StartPath").Value = value;
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public string Argument
		{
			get
			{
				return Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("Argument").Value;
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("Argument").Value = value;
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int WindowWidth
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("WindowWidth").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("WindowWidth").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int WindowHeight
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("WindowHeight").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("WindowHeight").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int XPos
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("XPos").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("XPos").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int YPos
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("YPos").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("YPos").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int WindowState
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, Id).Element("WindowState").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, Id).Element("WindowState").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public WindowsProgram(string ParentProfileName, int Id)
		{
			this.ParentProfileName = ParentProfileName;
			this.Id = Id;
		}
	}
}
