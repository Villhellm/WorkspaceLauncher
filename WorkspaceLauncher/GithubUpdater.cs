using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkspaceLauncher.ViewModels;
using System.Web;
using System.Windows;

namespace WorkspaceLauncher
{
    public class GithubUpdater
    {
		public static string ReleaseVersionURL = "https://raw.githubusercontent.com/Villhellm/WorkspaceLauncher/master/WorkspaceLauncher/CurrentRelease/Version.txt";
		public static string ReleaseDownloadURL = "https://github.com/Villhellm/WorkspaceLauncher/blob/master/WorkspaceLauncher/CurrentRelease/WorkspaceLauncher.exe?raw=true";
		static string MemeCacheBlockerUR = "?t=" + DateTime.Now.ToString().Replace(" ", "");
		string ApplicationLocation { get { return System.Reflection.Assembly.GetExecutingAssembly().Location; } }

		public static string GetWebString(string URL)
		{
			try
			{
				WebClient Checker = new WebClient();
				Checker.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
				return Checker.DownloadString(URL + MemeCacheBlockerUR);
			}
			catch(WebException ex)
			{
				string message = ex.Message;
				return "";
			}		
		}

		public static string LatestVersion { get { return GetWebString(ReleaseVersionURL); } }

		public bool VersionCompare(string VersionOriginal, string VersionToCheck)
		{
			int VO1 = Convert.ToInt32(VersionOriginal.Substring(0, VersionOriginal.IndexOf('.')));
			VersionOriginal = VersionOriginal.Substring(VersionOriginal.IndexOf('.') + 1);
			int VO2 = Convert.ToInt32(VersionOriginal.Substring(0, VersionOriginal.IndexOf('.')));
			VersionOriginal = VersionOriginal.Substring(VersionOriginal.IndexOf('.') + 1);
			int VO3 = Convert.ToInt32(VersionOriginal.Substring(0, VersionOriginal.IndexOf('.')));
			VersionOriginal = VersionOriginal.Substring(VersionOriginal.IndexOf('.') + 1);
			int VO4 = Convert.ToInt32(VersionOriginal);

			int VC1 = Convert.ToInt32(VersionToCheck.Substring(0, VersionToCheck.IndexOf('.')));
			VersionToCheck = VersionToCheck.Substring(VersionToCheck.IndexOf('.') + 1);
			int VC2 = Convert.ToInt32(VersionToCheck.Substring(0, VersionToCheck.IndexOf('.')));
			VersionToCheck = VersionToCheck.Substring(VersionToCheck.IndexOf('.') + 1);
			int VC3 = Convert.ToInt32(VersionToCheck.Substring(0, VersionToCheck.IndexOf('.')));
			VersionToCheck = VersionToCheck.Substring(VersionToCheck.IndexOf('.') + 1);
			int VC4 = Convert.ToInt32(VersionToCheck);

			if (VC1 > VO1)
				return true;
			else if (VC1 == VO1)
			{
				if (VC2 > VO2)
					return true;
				else if (VC2 == VO2)
				{
					if (VC3 > VO3)
						return true;
					else if (VC3 == VO3)
					{
						if (VC4 > VO4)
							return true;
					}
				}
			}

			return false;
		}

		public void LaunchUpdaterAsync()
		{
			BackgroundWorker UpdateChecker = new BackgroundWorker();
			UpdateChecker.DoWork += CheckForUpdate;
			UpdateChecker.RunWorkerAsync();
		}

		public void CheckForUpdate(object sender, DoWorkEventArgs e)
		{
			if (Configuration.Version != "" && VersionCompare(Configuration.Version, LatestVersion))
			{
				MessageBoxResult DR = MessageBox.Show("Version " + LatestVersion + " is available, would you like to update ?\n \n" + "Changes: ", "Update", MessageBoxButton.YesNo);
				if (DR == MessageBoxResult.Yes)
				{
					UpdateProgram();
				}
			}
		}

		public int LaunchUpdater()
		{
			if (Configuration.Version != "" && VersionCompare(Configuration.Version, LatestVersion))
			{
				MessageBoxResult DR = MessageBox.Show("Version " + LatestVersion + " is available, would you like to update ?\n \n" + "Changes: ", "Update", MessageBoxButton.YesNo);
				if (DR == MessageBoxResult.Yes)
				{
					UpdateProgram();
				}
				return 1;
			}
			return 0;
		}

		public void DownloadInternetFile(string sourceURL, string destinationPath)
		{
			long fileSize = 0;
			int bufferSize = 1024;
			bufferSize *= 1000;
			long existLen = 0;

			FileStream saveFileStream;

			if (File.Exists(destinationPath))
			{
				File.Delete(destinationPath);
			}

			saveFileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

			HttpWebRequest httpReq;
			HttpWebResponse httpRes;
			httpReq = (HttpWebRequest)HttpWebRequest.Create(sourceURL);
			httpReq.AddRange((int)existLen);
			Stream resStream;
			httpRes = (HttpWebResponse)httpReq.GetResponse();
			resStream = httpRes.GetResponseStream();

			fileSize = httpRes.ContentLength;

			int byteSize;
			byte[] downBuffer = new byte[bufferSize];

			while ((byteSize = resStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
			{
				saveFileStream.Write(downBuffer, 0, byteSize);
			}
		}

		private void UpdateProgram()
		{
			DownloadInternetFile(ReleaseDownloadURL + MemeCacheBlockerUR, ApplicationLocation + "t");
			SelfDestruct();
		}

		private void SelfDestruct()
		{
			if (File.Exists(ApplicationLocation + "t"))
			{
				UpdateVersion();
				string ProgName = Path.GetFileName(ApplicationLocation);
				string ProgPathWithName = ApplicationLocation;
				string ProgPath = Path.GetDirectoryName(ApplicationLocation);
				Process.Start("cmd.exe", "/C timeout 3 & Del \"" + ProgPathWithName + "\"& RENAME \"" + ProgPathWithName + "t\" " + "\"" + ProgName + "\"" + " & start \"" + ProgPath + "\" \"" + ProgName + "\"");
				Environment.Exit(0);
			}
		}

		public void UpdateVersion()
		{
			Configuration.Version = LatestVersion;
		}

	}
}
