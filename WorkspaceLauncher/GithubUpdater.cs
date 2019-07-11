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
using WorkspaceLauncher.Models;

namespace WorkspaceLauncher
{
    public class GithubUpdater
    {
		public static string ReleaseVersionURL = "https://raw.githubusercontent.com/Villhellm/WorkspaceLauncher/master/WorkspaceLauncher/CurrentRelease/Version.txt";
		public static string ReleaseDownloadURL = "https://github.com/Villhellm/WorkspaceLauncher/blob/master/WorkspaceLauncher/CurrentRelease/WorkspaceLauncher.exe?raw=true";
		static string MemeCacheBlockerUR = "?t=" + DateTime.Now.ToString().Replace(" ", "");
		string ApplicationLocation { get { return System.Reflection.Assembly.GetExecutingAssembly().Location; } }

		public static int LatestVersion
		{
			get
			{
				try
				{
					WebClient checker = new WebClient();
					checker.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
					string download = checker.DownloadString(ReleaseVersionURL);
					return 0;
				}
				catch (WebException ex)
				{
					string message = ex.Message;
					return -1;
				}
			}
		}

		public void LaunchUpdaterAsync()
		{
			BackgroundWorker UpdateChecker = new BackgroundWorker();
			UpdateChecker.DoWork += CheckForUpdate;
			UpdateChecker.RunWorkerAsync();
		}

		public void CheckForUpdate(object sender, DoWorkEventArgs e)
		{
			if (Configuration.Instance.Version != -1 && (LatestVersion > Configuration.Instance.Version))
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
			if (Configuration.Instance.Version != -1 && (LatestVersion > Configuration.Instance.Version))
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
			Configuration.Instance.Version = LatestVersion;
		}

	}
}
