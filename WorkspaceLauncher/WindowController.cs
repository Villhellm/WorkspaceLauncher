using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkspaceLauncher.Models;

namespace WorkspaceLauncher
{
	public static class WindowController
	{
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll", SetLastError = true)]
		static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WINDOWPLACEMENT
		{
			public int length;
			public int flags;
			public int showCmd;
			public System.Windows.Point ptMinPosition;
			public System.Windows.Point ptMaxPosition;
			public System.Windows.Shapes.Rectangle rcNormalPosition;
		}

		//Hide = 0,
		//Normal = 1,
		//Minimized = 2,
		//Maximized = 3,
		public static void PositionWindow(string ProcessName, int xPos, int yPos, int wWidth, int wHeight, int Status)
		{
			IntPtr Prog;
			ProcessName = ProcessName.ToLower();
			Process[] ProcessList = Process.GetProcesses();
			foreach (Process Prcs in ProcessList)
			{
				if (!String.IsNullOrEmpty(Prcs.MainWindowTitle))
				{
					if (Prcs.ProcessName.ToLower().Contains(ProcessName))
					{
						Prog = Prcs.MainWindowHandle;
						if(Status == 2)
						{
							MoveWindow(Prog, xPos, yPos, wWidth, wHeight, true);
							ShowWindow(Prog, Status);
						}
						else
						{
							ShowWindow(Prog, Status);
							MoveWindow(Prog, xPos, yPos, wWidth, wHeight, true);
						}
						WINDOWPLACEMENT Checker = new WINDOWPLACEMENT();
						GetWindowPlacement(Prog, ref Checker);
					}
				}
			}
		}

		public static int WindowXPosition(Process Proc)
		{
			IntPtr ProcHndl = GetWindowHandleByProcess(Proc.ProcessName);
			RECT Wnd;
			GetWindowRect(ProcHndl, out Wnd);
			return Wnd.Left;
		}

		public static int WindowYPosition(Process Proc)
		{
			IntPtr ProcHndl = GetWindowHandleByProcess(Proc.ProcessName);
			RECT Wnd;
			GetWindowRect(ProcHndl, out Wnd);
			return Wnd.Top;
		}

		public static int GetWindowHeight(Process Proc)
		{
			IntPtr ProcHndl = GetWindowHandleByProcess(Proc.ProcessName);
			RECT Wnd;
			GetWindowRect(ProcHndl, out Wnd);
			return Wnd.Bottom - Wnd.Top;
		}

		public static int GetWindowWidth(Process Proc)
		{
			IntPtr ProcHndl = GetWindowHandleByProcess(Proc.ProcessName);
			RECT Wnd;
			GetWindowRect(ProcHndl, out Wnd);
			return Wnd.Right - Wnd.Left;
		}

		public static int GetWindowStatus(Process Proc)
		{
			IntPtr ProcHndl = GetWindowHandleByProcess(Proc.ProcessName);
			WINDOWPLACEMENT Current = new WINDOWPLACEMENT();
			GetWindowPlacement(ProcHndl, ref Current);
			return Current.showCmd;
		}

		public static IntPtr GetWindowHandleByCaption(string CaptionText)
		{
			IntPtr Prog = new IntPtr();
			Process[] ProcessList = Process.GetProcesses();
			foreach (Process Prcs in ProcessList)
			{
				if (!String.IsNullOrEmpty(Prcs.MainWindowTitle))
				{
					if (Prcs.MainWindowTitle.ToLower().Contains(CaptionText))
					{
						return Prcs.MainWindowHandle;
					}
				}
			}
			return Prog;
		}

		public static IntPtr GetWindowHandleByProcess(string ProcessName)
		{
			IntPtr Prog = new IntPtr();
			ProcessName = ProcessName.ToLower();
			Process[] ProcessList = Process.GetProcesses();
			foreach (Process Prcs in ProcessList)
			{
				if (!String.IsNullOrEmpty(Prcs.MainWindowTitle))
				{
					if (Prcs.ProcessName.ToLower().Contains(ProcessName))
					{
						return Prcs.MainWindowHandle;
					}
				}
			}
			return Prog;
		}

		public static void LaunchAll(List<WindowsProgram> Programs)
		{
			foreach (WindowsProgram Prog in Programs)
			{
				try
				{
					if (!IsProcessOpen(Prog.ProcessName))
					{
						string ProgPath = Path.GetDirectoryName(Prog.StartPath);
						string ProgName = Path.GetFileName(Prog.StartPath);
						string Arguments = Prog.Argument;
						ProcessStartInfo LaunchInfo = new ProcessStartInfo();
						LaunchInfo.WorkingDirectory = ProgPath;
						LaunchInfo.FileName = ProgName;
						LaunchInfo.Arguments = Arguments;
						Process.Start(LaunchInfo);
					}
				}
				catch (InvalidOperationException)
				{

				}
			}
		}

		public static void PositionAll(List<WindowsProgram> Programs)
		{
			foreach (WindowsProgram Prog in Programs)
			{
				try
				{
					PositionWindow(Prog.ProcessName, Prog.XPos, Prog.YPos, Prog.WindowWidth, Prog.WindowHeight, Prog.WindowState);
				}
				catch (InvalidOperationException)
				{

				}
			}
		}

		private static void WaitForLaunch(List<WindowsProgram> Programs)
		{
			bool IsOpen = false;
			foreach (WindowsProgram Elmnt in Programs)
			{
				while (!IsOpen)
				{
					Process[] ProcessList = Process.GetProcesses();
					foreach (Process Proc in ProcessList)
					{
						if (!String.IsNullOrEmpty(Proc.MainWindowTitle))
						{
							if (Proc.ProcessName == Elmnt.ProcessName)
							{
								IsOpen = true;
							}
						}
					}
				}
				IsOpen = false;
				Thread.Sleep(100);
			}

		}

		public static void LaunchAndPositionAll(List<WindowsProgram> Programs)
		{
			LaunchAll(Programs);
			WaitForLaunch(Programs);
			PositionAll(Programs);
		}

		private static bool IsProcessOpen(string ProcessName)
		{
			Process[] OpenProcesses = Process.GetProcesses();
			Process[] ProcessList = Process.GetProcesses();
			foreach (Process Proc in ProcessList)
			{
				if (!String.IsNullOrEmpty(Proc.MainWindowTitle))
				{
					if (Proc.ProcessName == ProcessName)
					{
						return true;
					}
				}
			}
			return false;

		}

	}
}
