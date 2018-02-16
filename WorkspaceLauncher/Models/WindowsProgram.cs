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
		public string ProcessName { get; set; }

		public string StartPath
		{
			get
			{
				return Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("StartPath").Value;
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("StartPath").Value = value;
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public string Argument
		{
			get
			{
				return Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("Argument").Value;
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("Argument").Value = value;
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int WindowWidth
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("WindowWidth").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("WindowWidth").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int WindowHeight
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("WindowHeight").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("WindowHeight").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int XPos
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("XPos").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("XPos").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int YPos
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("YPos").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("YPos").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public int WindowState
		{
			get
			{
				return Convert.ToInt32(Configuration.Program(Configuration.xConfiguration, ParentProfileName, ProcessName).Element("WindowState").Value);
			}
			set
			{
				XDocument xConfig = Configuration.xConfiguration;
				Configuration.Program(xConfig, ParentProfileName, ProcessName).Element("WindowState").Value = value.ToString();
				xConfig.Save(Configuration.ConfigurationFile);
			}
		}

		public WindowsProgram(string ParentProfileName, string ProcessName)
		{
			this.ParentProfileName = ParentProfileName;
			this.ProcessName = ProcessName;
		}
	}
}
