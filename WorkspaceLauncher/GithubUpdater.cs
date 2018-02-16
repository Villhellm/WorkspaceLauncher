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

namespace WorkspaceLauncher
{
    public class GithubUpdater
    {
		public static string ReleaseURL = "https://raw.githubusercontent.com/Villhellm/QuickStream/master/StreamStarter/Version.txt";
		static string MemeCacheBlockerUR = "?t=" + DateTime.Now.ToString().Replace(" ", "");
		string ApplicationLocation { get { return ""; } }


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

		public static string LatestVersion
		{
			get
			{
				return GetWebString(ReleaseURL);
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
			if (Configuration.Version != "" && LatestVersion != Configuration.Version)
			{
				ConfirmationDialogViewModel DR = new ConfirmationDialogViewModel("Update", "Version " + LatestVersion + " is available, would you like to update?\n \n" + "Changes: ");
				if (DR.DialogResult == 1)
				{
					UpdateProgram();
				}
			}
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

			saveFileStream = new FileStream(destinationPath, System.IO.FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

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
			//DownloadInternetFile(ExecutableDownloadURL + MemeCacheBlockerUR, ApplicationLocation + "t");
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
